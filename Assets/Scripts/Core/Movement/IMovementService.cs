using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public interface IMovementService
    {
        void StartMove(IMovable movable, Vector3 movingPosition);
        
        void Continue(params IMovable[] movableObjects);
        
        void Stop(params IMovable[] movableObjects);
    }
}