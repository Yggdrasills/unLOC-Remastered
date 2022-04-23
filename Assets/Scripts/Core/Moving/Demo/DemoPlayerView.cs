using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Core.Moving.Demo
{
    public class DemoPlayerView : MonoBehaviour, IMovable
    {
        [SerializeField] private float _movingDuration = 2;
        private Tween _activeTween;

        public bool IsActive { get; set; }
        public bool IsMoving { get; private set; }
        
        UniTask IMovable.MoveToPointAsync(Vector3 point, CancellationToken token)
        {
            if (!IsActive) return UniTask.CompletedTask;

            MoveHorizontalAsync(point.x).Forget();
            
            return UniTask.CompletedTask;
        }

        void IMovable.ContinueMoving()
        {
            if (!_activeTween.active && _activeTween != null)
            {
                _activeTween.Play();
            }
        }

        void IMovable.PauseMoving()
        {
            if (!IsMoving) return;
            
            if (_activeTween.active && _activeTween != null)
            {
                _activeTween.Pause();
            }
        }

        void IMovable.StopMoving()
        {
            if (_activeTween.active && _activeTween != null)
            {
                _activeTween.Kill();
            }
        }

        private async UniTask MoveHorizontalAsync(float horizontalPoint)
        {
            transform.localScale = horizontalPoint < transform.position.x ?
                new Vector3(-1,transform.localScale.y,1) : new Vector3(1,transform.localScale.y,1);

            _activeTween = transform.DOMoveX(horizontalPoint, _movingDuration);

            IsMoving = true;

            await _activeTween.AsyncWaitForCompletion();

            IsMoving = false;
        }
    }
}