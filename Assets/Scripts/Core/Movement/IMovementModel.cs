using System;

namespace SevenDays.unLOC.Core.Movement
{
    public interface IMovementModel
    {
        event Action StartMove;
        event Action StopMove;
        bool IsMoving { get; }
        bool IsMovingToPoint { get; }
        float MovingSpeed { get; }
    }
}