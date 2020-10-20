using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tanks_Game.Interface;

namespace Tanks_Game.Models
{
    class UserTank : Prototype
    {
        // напрямок руху
        public MoveVector CurrentMoveVector { get; set; }
        // час очікування вводу для руху юзером
        const int timeDelay = 10;
        // початкові координати
        const int rowStartPosition = 23;
        const int columnStartPosition = 40;
        // конструктор з параметрами
        public UserTank()
        {
            coordinate.Row = rowStartPosition;
            coordinate.Column = columnStartPosition;
            previousCoordinate = coordinate;

            CurrentMoveVector = MoveVector.up;
            IsAlive = true;
            IsJustCreate = true;
            Look = 'W';
        }

        // опис методу для руху юзером (очікується кілька млсек на ввід якщо ні продовжується програма)
        [DllImport("msvcrt")]
        static extern char _getch();

        static async Task<char> ReadCharTimeout(int delay)
        {
            var read = Task.Run(() => { return _getch(); });

            if (await Task.WhenAny(read, Task.Delay(delay)) == read)
            {
                return read.Result;
            }
            else
            {
                return '0';
            }
        }
        // функціонал танка або рух або постріл
        public void MoveOrShoot()
        {
            if (Game.health <= 0)
            {
                Game.isGame = false;
                return;
            }
            previousCoordinate = coordinate;

            char moveOrShoot;
            using (var temp = ReadCharTimeout(timeDelay))
            {
                moveOrShoot = temp.Result;
                temp.Dispose();
            }
            // на випадок включеного Caps Lock
            switch (moveOrShoot)
            {
                case 'W':
                    moveOrShoot = 'w';
                    break;
                case 'S':
                    moveOrShoot = 's';
                    break;
                case 'A':
                    moveOrShoot = 'a';
                    break;
                case 'D':
                    moveOrShoot = 'd';
                    break;
                default:
                    break;
            }

            switch (moveOrShoot)
            {
                case 'w':
                    CurrentMoveVector = MoveVector.up;
                    break;
                case 's':
                    CurrentMoveVector = MoveVector.down;
                    break;
                case 'a':
                    CurrentMoveVector = MoveVector.left;
                    break;
                case 'd':
                    CurrentMoveVector = MoveVector.right;
                    break;
                default:
                    break;
            }

            if (moveOrShoot == 'w' && Game.map[coordinate.Row - 1, coordinate.Column] == ' ')
                --coordinate.Row;
            else if (moveOrShoot == 's' && Game.map[coordinate.Row + 1, coordinate.Column] == ' ')
                ++coordinate.Row;
            else if (moveOrShoot == 'a' && Game.map[coordinate.Row, coordinate.Column - 1] == ' ')
                --coordinate.Column;
            else if (moveOrShoot == 'd' && Game.map[coordinate.Row, coordinate.Column + 1] == ' ')
                ++coordinate.Column;
            else if (moveOrShoot == 'w' && Game.map[coordinate.Row - 1, coordinate.Column] == '*')
            {
                Game.health -= 20;
                --coordinate.Row;
            }
            else if (moveOrShoot == 's' && Game.map[coordinate.Row + 1, coordinate.Column] == '*')
            {
                Game.health -= 20;
                ++coordinate.Row;
            }
            else if (moveOrShoot == 'a' && Game.map[coordinate.Row, coordinate.Column - 1] == '*')
            {
                Game.health -= 20;
                --coordinate.Column;
            }
            else if (moveOrShoot == 'd' && Game.map[coordinate.Row, coordinate.Column + 1] == '*')
            {
                Game.health -= 20;
                ++coordinate.Column;
            }
            else if (moveOrShoot == ' ')
                Shoot();
            else if (moveOrShoot == 'q' || moveOrShoot == 'Q')
                Game.isGame = false;

            OverwritingMap();
        }
        // перезапис карти
        public void OverwritingMap()
        {
            Game.map[previousCoordinate.Row, previousCoordinate.Column] = ' ';
            if (Game.health > 0)
                Game.map[coordinate.Row, coordinate.Column] = Look;
            else
                Game.map[coordinate.Row, coordinate.Column] = ' ';
        }
        // вистріл
        public override void Shoot()
        {
            if (CurrentMoveVector == MoveVector.up)
            {
                Game.bullets.Add(new Bullet(coordinate.Row - 1, coordinate.Column, CurrentMoveVector, '+'));

            }
            if (CurrentMoveVector == MoveVector.down)
            {
                Game.bullets.Add(new Bullet(coordinate.Row + 1, coordinate.Column, CurrentMoveVector, '+'));

            }
            if (CurrentMoveVector == MoveVector.left)
            {
                Game.bullets.Add(new Bullet(coordinate.Row, coordinate.Column - 1, CurrentMoveVector, '+'));

            }
            if (CurrentMoveVector == MoveVector.right)
            {
                Game.bullets.Add(new Bullet(coordinate.Row, coordinate.Column + 1, CurrentMoveVector, '+'));

            }
        }
    }
}