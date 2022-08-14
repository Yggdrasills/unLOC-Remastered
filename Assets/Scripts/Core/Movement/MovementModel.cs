using System;
using System.Threading;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public class MovementModel
    {
        public const int RightSideValue = 1;
        public const int LeftSideValue = -1;

        public event Action StartMove = delegate { };
        public event Action StopMove = delegate { };
        public bool IsMoving { get; set; }
        public bool IsMovingToPoint { get; set; }

        public Tween ActiveTween { get; set; }

        public CancellationTokenSource MovingToken { get; private set; }

        public Vector2 Range { get; }

        public float MovingSpeed { get; }

        public MovementModel(Vector2 range, float movingSpeed)
        {
            Range = range;
            MovingSpeed = movingSpeed;
        }


        public void UpdateToken()
        {
            MovingToken?.Cancel();
            MovingToken?.Dispose();
            MovingToken = new CancellationTokenSource();
        }

        public void OnStop()
        {
            StopMove?.Invoke();
        }

        public void OnMove()
        {
            StartMove?.Invoke();
        }
    }
}