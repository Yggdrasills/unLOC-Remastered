using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Moving.Demo
{
    public class DemoPlayerView : MonoBehaviour, IMovable, ITickable
    {
        [SerializeField] private float _movingDuration = 2;
        [SerializeField] private float _movingSpeed = 2;
        private Tween _activeTween;

        public bool IsActive { get; set; }
        public bool IsMoving { get; private set; }
        
        UniTask IMovable.MoveToPointAsync(Vector3 point, CancellationToken token)
        {
            if (!IsActive) return UniTask.CompletedTask;

            MoveHorizontalAsync(point.x).Forget();
            
            return UniTask.CompletedTask;
        }
        
        void ITickable.Tick()
        {
            if (!IsActive) return;
            
            if (IsMoving) return;
            
            var translation = Input.GetAxis("Horizontal") * _movingSpeed;

            if (translation.Equals(0)) return;
            
            transform.localScale = translation < 0
                ? new Vector3(-1, transform.localScale.y, 1)
                : new Vector3(1, transform.localScale.y, 1);
                
            translation *= Time.deltaTime;

            transform.Translate(translation, 0, 0);
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