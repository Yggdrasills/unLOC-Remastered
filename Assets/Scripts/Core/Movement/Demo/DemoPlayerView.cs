using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class DemoPlayerView : MonoBehaviour, IInitializable, IMovable, ITickable
    {
        private const int RightSideValue = 1;
        private const int LeftSideValue = -1;
        
        [SerializeField] private float _movingDuration = 2;
        [SerializeField] private float _movingSpeed = 2;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Vector2 _playerRootMovableRange;

        private Tween _activeTween;
        private string _idleAnimation;
        private string _walkingAnimations;
        private string[] _specialIdleAnimations;

        public bool IsActive { get; set; }
        public bool IsMoving { get; private set; }

        void IInitializable.Initialize()
        {
            _idleAnimation = "PlayerIdleLeft";
            _specialIdleAnimations = new [] {"PlayerIdleLeft_spec", "PlayerIdleLeft_spec2"};
            _walkingAnimations = "PlayerWalkRight";
        }

        UniTask IMovable.MoveToPointAsync(Vector3 point, CancellationToken token)
        {
            if (!IsActive) return UniTask.CompletedTask;

            MoveHorizontalAsync(point.x).Forget();

            return UniTask.CompletedTask;
        }

        void ITickable.Tick()
        {
            if (!IsActive) return;

            if (IsMoving)
            {
                _playerAnimator.Play(_idleAnimation);
                return;
            }

            var translation = Input.GetAxis("Horizontal") * _movingSpeed;

            if (translation.Equals(0))
            {
                _playerAnimator.Play(_idleAnimation);
                return;
            }
            
            if (transform.position.x <= _playerRootMovableRange.x && translation < 0)
            {
                _playerAnimator.Play(_idleAnimation);
                return;
            }

            if (transform.position.x >= _playerRootMovableRange.y && translation > 0)
            {
                _playerAnimator.Play(_idleAnimation);
                return;
            }

            RotatePlayer(translation, 0);

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
            RotatePlayer(horizontalPoint, transform.position.x);

            if (!IsInRange(horizontalPoint, out var inRangePoint))
            {
                horizontalPoint = inRangePoint;
            }

            _activeTween = transform.DOMoveX(horizontalPoint, _movingDuration);

            IsMoving = true;

            await _activeTween.AsyncWaitForCompletion();

            IsMoving = false;
        }

        private void RotatePlayer(float horizontalPoint, float comparableValue)
        {
            var rotationValue = horizontalPoint < comparableValue ? LeftSideValue : RightSideValue;
            
            transform.localScale = new Vector3(rotationValue, transform.localScale.y, 1);

            _playerAnimator.Play(_walkingAnimations);
        }

        private bool IsInRange(float horizontalPoint, out float inRangePoint)
        {
            if (horizontalPoint <= _playerRootMovableRange.x)
            {
                inRangePoint = _playerRootMovableRange.x;
                return false;
            }
            
            if (horizontalPoint >= _playerRootMovableRange.y)
            {
                inRangePoint = _playerRootMovableRange.y;
                return false;
            }

            inRangePoint = horizontalPoint;
            return true;
        }
    }
}