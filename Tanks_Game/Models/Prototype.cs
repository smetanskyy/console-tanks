using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Game.Models
{
    // структура координат
    struct Coordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
    // опис прототипа
    abstract class Prototype
    {
        // щойно створений обєкт (правда)
        protected bool IsJustCreate;
        // поточні координати
        protected Coordinate coordinate;
        // попередні координати
        protected Coordinate previousCoordinate;
        // чи обєкт живий
        public bool IsAlive { get; set; }
        // вигляд
        protected char Look { get; set; }
        // перевірка чи ще живий
        public virtual void CheckIsAlive()
        {
            if (IsJustCreate)
            {
                IsJustCreate = false;
                return;
            }
            if (IsAlive)
            {
                if (Game.map[coordinate.Row, coordinate.Column] != Look)
                {
                    IsAlive = false;
                    coordinate.Row = 0;
                    coordinate.Column = 0;
                }
            }
        }
        // абстрактне поле постріл
        public abstract void Shoot();
    }
}
