using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace SevenDays.unLOC.Core.Moving
{
    public class MovingService : IMovingService
    {
        public void Start(IMovable movableObject, Vector3 position)
        {
            movableObject.MoveToPointAsync(position, CancellationToken.None);
        }

        public void Continue(params IMovable[] movableObjects)
        {
            DoMovingAction(movableObjects, movable =>
            {
                movable.ContinueMoving();
            });
        }

        public void Stop(params IMovable[] movableObjects)
        {
            DoMovingAction(movableObjects, movable =>
            {
                movable.StopMoving();
            });
        }

        private static void DoMovingAction(IEnumerable<IMovable> movableObjects, Action<IMovable> movingAction)
        {
            foreach (var movable in movableObjects)
            {
                movingAction.Invoke(movable);
            }
        }
    }
}