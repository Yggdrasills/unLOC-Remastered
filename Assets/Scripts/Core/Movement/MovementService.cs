using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    // review: лишний класс. Никто не должен говорить персонажу что можно передвигаться.
    // review: есть контроллер, отвечающий за передвижение и он сам определяет когда персонажу нужно двигаться
    public class MovementService : IMovementService
    {
        private CancellationTokenSource _movingToken;
        
        public void StartMove(IMovable movableObject, Vector3 position)
        {
            movableObject.IsActive = true;
            
            _movingToken?.Cancel();
            // review: не диспоузится
            _movingToken = new CancellationTokenSource();
            
            movableObject.MoveToPointAsync(position, _movingToken.Token);
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