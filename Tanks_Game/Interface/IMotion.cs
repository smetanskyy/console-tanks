namespace Tanks_Game.Interface
{
    // перечислення направлення руху
    enum MoveVector
    {
        up, down, left, right
    }

    // інтерфейс для руху(переміщення)
    interface IMotion
    {
        // показує напрям руху
        MoveVector CurrentMoveVector { get; set; }
        //власне сам рух
        void Move();
        //перезапис карти
        void OverwritingMap();
    }
}
