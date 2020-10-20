using Tanks_Game.Interface;

namespace Tanks_Game.Models
{
    class Bullet : Prototype, IMotion
    {
        // напрям руху
        public MoveVector CurrentMoveVector { get; set; }
        // конструктор з параметрами
        public Bullet(int row, int column, MoveVector vector, char look)
        {
            // встановлення координат кулі
            coordinate.Row = row;
            coordinate.Column = column;
            // у точці створення кулі попередні координати і поточні координати збігаються
            previousCoordinate = coordinate;
            // задаємо напрям руху кулі
            CurrentMoveVector = vector;
            // куля "жива"
            IsAlive = true;
            // куля щойно створена
            IsJustCreate = true;
            // вигляд кулі
            Look = look;
            // наносимо кулю на карту
            OverwritingMap();
        }
        // рух кулі
        public void Move()
        {
            // перевіряємо чи куля ще "жива"
            CheckIsAlive();
            // якщо ні виходимо
            if (IsAlive == false)
                return;
            // перезаписуємо попередні координати
            previousCoordinate = coordinate;

            // взалежності від напрямку руху здійснюємо переміщення на карті
            if (CurrentMoveVector == MoveVector.up)
            {
                --coordinate.Row;
            }
            if (CurrentMoveVector == MoveVector.down)
            {
                ++coordinate.Row;
            }
            if (CurrentMoveVector == MoveVector.left)
            {
                --coordinate.Column;
            }
            if (CurrentMoveVector == MoveVector.right)
            {
                ++coordinate.Column;
            }
            // перезаписуємо кулю на карті
            OverwritingMap();
        }
        // перезапис карти
        public void OverwritingMap()
        {
            // якщо куля тільки створена не потрібно "стирати" її з карти
            // куля проходить над "річкою" але тимчасово зникає з екрана
            if (!IsJustCreate && Game.map[previousCoordinate.Row, previousCoordinate.Column] != '{' &&
                Game.map[previousCoordinate.Row, previousCoordinate.Column] != '}')

                Game.map[previousCoordinate.Row, previousCoordinate.Column] = ' ';
            // взаємодія кулі з мітками на карті
            if (Game.map[coordinate.Row, coordinate.Column] == ' ')
                Game.map[coordinate.Row, coordinate.Column] = Look;
            else if (Game.map[coordinate.Row, coordinate.Column] == '{')
            {
                Game.map[coordinate.Row, coordinate.Column] = '{';
            }
            else if (Game.map[coordinate.Row, coordinate.Column] == '}')
            {
                Game.map[coordinate.Row, coordinate.Column] = '}';
            }
            else
            {
                IsAlive = false;
                if (Game.map[coordinate.Row, coordinate.Column] == '*' || Game.map[coordinate.Row, coordinate.Column] == '+' ||
                    Game.map[coordinate.Row, coordinate.Column] == 'Y' || Game.map[coordinate.Row, coordinate.Column] == '[' ||
                    Game.map[coordinate.Row, coordinate.Column] == ']')
                    Game.map[coordinate.Row, coordinate.Column] = ' ';
                else if (Game.map[coordinate.Row, coordinate.Column] == '$')
                {
                    Game.isGame = false;
                    Game.map[coordinate.Row, coordinate.Column] = ' ';
                }
                else if (Game.map[coordinate.Row, coordinate.Column] == 'W')
                    Game.health -= 20;
                else if (Game.map[coordinate.Row, coordinate.Column] == 'U' && Look == '+')
                    Game.map[coordinate.Row, coordinate.Column] = ' ';
            }
            if (Game.health <= 0)
                Game.isGame = false;
        }

        // перевіряємо чи куля ще "жива"
        public override void CheckIsAlive()
        {
            if (IsJustCreate)
            {
                IsJustCreate = false;
                return;
            }
            if (IsAlive)
            {
                if (Game.map[coordinate.Row, coordinate.Column] != Look && Game.map[coordinate.Row, coordinate.Column] != '{' &&
                    Game.map[coordinate.Row, coordinate.Column] != '}')
                {
                    IsAlive = false;
                    coordinate.Row = 0;
                    coordinate.Column = 0;
                }
            }
        }

        // куля сама не стріляє
        public override sealed void Shoot()
        {
            return;
        }
    }
}