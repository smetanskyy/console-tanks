using System;
using Tanks_Game.Interface;

namespace Tanks_Game.Models
{
    // клас Ворог наслідується від Прототипа і реалізує інтерфейс
    class Enemy : Prototype, IMotion
    {
        // напрям руху
        public MoveVector CurrentMoveVector { get; set; }
        // для рандомних значень (напрямки руху будуть задаватися рандомно)
        public static Random rand = new Random();

        // конструктор з параметрами
        public Enemy(int row, int column)
        {
            // встановлюємо координати
            coordinate.Row = row;
            coordinate.Column = column;
            previousCoordinate = coordinate;

            // задаємо напрямок руху - вниз
            CurrentMoveVector = MoveVector.down;
            // "живий"
            IsAlive = true;
            // щойно створений
            IsJustCreate = true;
            //вигляд на карті
            Look = 'U';
        }

        // один рух(крок)
        public void Move()
        {
            // перевіряємо чи ще "живий"
            CheckIsAlive();
            // якщо "мертвий" виходимо
            if (IsAlive == false)
                return;
            // перезаписуємо попередні координати
            previousCoordinate = coordinate;
            // для вибору напрямку руху
            MoveVector tryMove;
            // створюємо лічильник, щоб не зациклився
            int counterForLoop = 0;
            while (counterForLoop != 10)
            {
                // якщо є можливість танк рухається в одному напрямку
                if (counterForLoop == 0)
                    tryMove = CurrentMoveVector;
                // якщо нема можливості рухатися міняємо напрямок
                else
                {
                    tryMove = (MoveVector)rand.Next(4);
                    CurrentMoveVector = tryMove;
                }
                // збільшуємо лічильник
                ++counterForLoop;

                // перевіряємо можливості руху
                if (tryMove == MoveVector.up && Game.map[coordinate.Row - 1, coordinate.Column] == ' ')
                {
                    --coordinate.Row;
                    break;
                }
                else if (tryMove == MoveVector.down && Game.map[coordinate.Row + 1, coordinate.Column] == ' ')
                {
                    ++coordinate.Row;
                    break;
                }
                else if (tryMove == MoveVector.left && Game.map[coordinate.Row, coordinate.Column - 1] == ' ')
                {
                    --coordinate.Column;
                    break;
                }
                else if (tryMove == MoveVector.right && Game.map[coordinate.Row, coordinate.Column + 1] == ' ')
                {
                    ++coordinate.Column;
                    break;
                }
                // якщо натикається на кулю юзера "вмирає"
                else if (tryMove == MoveVector.up && Game.map[coordinate.Row - 1, coordinate.Column] == '+')
                {
                    IsAlive = false;
                    --coordinate.Row;
                    break;
                }
                else if (tryMove == MoveVector.down && Game.map[coordinate.Row + 1, coordinate.Column] == '+')
                {
                    IsAlive = false;
                    ++coordinate.Row;
                    break;
                }
                else if (tryMove == MoveVector.left && Game.map[coordinate.Row, coordinate.Column - 1] == '+')
                {
                    IsAlive = false;
                    --coordinate.Column;
                    break;
                }
                else if (tryMove == MoveVector.right && Game.map[coordinate.Row, coordinate.Column + 1] == '+')
                {
                    IsAlive = false;
                    ++coordinate.Column;
                    break;
                }
            }
            // перезапис карти
            OverwritingMap();
        }

        // перезапис карти
        public void OverwritingMap()
        {
            Game.map[previousCoordinate.Row, previousCoordinate.Column] = ' ';
            if (IsAlive)
                Game.map[coordinate.Row, coordinate.Column] = Look;
            else
                Game.map[coordinate.Row, coordinate.Column] = ' ';
        }

        // постріл
        public override void Shoot()
        {
            if (CurrentMoveVector == MoveVector.up)
                Game.bullets.Add(new Bullet(coordinate.Row - 1, coordinate.Column, CurrentMoveVector, '*'));
            if (CurrentMoveVector == MoveVector.down)
                Game.bullets.Add(new Bullet(coordinate.Row + 1, coordinate.Column, CurrentMoveVector, '*'));
            if (CurrentMoveVector == MoveVector.left)
                Game.bullets.Add(new Bullet(coordinate.Row, coordinate.Column - 1, CurrentMoveVector, '*'));
            if (CurrentMoveVector == MoveVector.right)
                Game.bullets.Add(new Bullet(coordinate.Row, coordinate.Column + 1, CurrentMoveVector, '*'));
        }
    }
}
