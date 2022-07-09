using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using SevenDays.unLOC.Core.Animations;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Core.Movement
{
    public class PlayerView : MonoBehaviour, IMovable
    {
        [field: SerializeField]
        public AnimationCallBackView AnimationCallBackView { get; private set; }

        public event Action StartMove = delegate { };

        public event Action Stay = delegate { };

        public Animator PlayerAnimator => _playerAnimator;

        [SerializeField]
        private float _movingSpeed = 2;

        [SerializeField]
        private Animator _playerAnimator;

        private const int RightSideValue = 1;

        private const int LeftSideValue = -1;

        private Vector2 _playerRootMovableRange;

        private Tween _activeTween;

        public bool IsActive { get; set; }

        public bool IsMoving { get; private set; }

        [Inject]
        public void Construct(TapZoneView tapZoneView)
        {
            _playerRootMovableRange = new Vector2(-1, 1) * (tapZoneView.Collider2D.size.x / 2 - 1);
        }

        UniTask IMovable.MoveToPointAsync(Vector3 point, CancellationToken token)
        {
            if (!IsActive) return UniTask.CompletedTask;

            MoveHorizontalAsync(point.x, token).Forget();

            return UniTask.CompletedTask;
        }

        void IMovable.Move(float horizontalDirection)
        {
            if (!IsActive) return;

            var translation = horizontalDirection * _movingSpeed;

            if (IsMoving)
                StopMoving();

            if (transform.position.x >= _playerRootMovableRange.y && translation > 0 ||
                transform.position.x <= _playerRootMovableRange.x && translation < 0)
            {
                Stay.Invoke();
                return;
            }

            RotatePlayer(translation, 0);

            translation *= Time.deltaTime;

            transform.Translate(translation, 0, 0);
            StartMove.Invoke();
        }

        void IMovable.ContinueMoving()
        {
            if (!_activeTween.active && _activeTween != null)
            {
                _activeTween.Play();
            }

            OnMove();
        }

        void IMovable.PauseMoving()
        {
            if (!IsMoving) return;

            if (_activeTween is { active: true })
            {
                _activeTween.Pause();
            }
        }

        public void StopMoving()
        {
            if (_activeTween is { active: true })
            {
                _activeTween.Kill();
            }

            OnStop();
        }

        private async UniTask MoveHorizontalAsync(float horizontalPoint, CancellationToken token)
        {
            var viewXPosition = transform.position.x;

            RotatePlayer(horizontalPoint, viewXPosition);

            var distance = Mathf.Abs(horizontalPoint - viewXPosition);

            OnMove();

            _activeTween = transform.DOMoveX(horizontalPoint, distance / _movingSpeed).SetEase(Ease.Linear);

            await _activeTween.AwaitForComplete(cancellationToken: token);

            OnStop();
        }

        private void RotatePlayer(float horizontalPoint, float comparableValue)
        {
            var rotationValue = horizontalPoint < comparableValue ? LeftSideValue : RightSideValue;

            transform.localScale = new Vector3(rotationValue, transform.localScale.y, 1);
        }

        private void OnStop()
        {
            Stay.Invoke();

            IsMoving = false;
        }

        private void OnMove()
        {
            StartMove.Invoke();

            IsMoving = true;
        }
    }
}