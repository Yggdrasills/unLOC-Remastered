using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using SaveSystem;

using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player.Animations;
using SevenDays.unLOC.Core.Player.Animations.Config;

using ToolBox.Serialization;

using UnityEngine;

using Utils;

namespace SevenDays.unLOC.Core.Player
{
    public class PlayerView : MonoBehaviour, IMovable, ISavableMono
    {
        [field: SerializeField]
        public AnimationCallBackView AnimationCallBackView { get; private set; }

        [field: SerializeField]
        public AnimationConfig AnimationConfig { get; private set; }

        [field: SerializeField]
        public CharacterScale CharacterScale { get; private set; }

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

        public void SetRange(TapZoneView tapZoneView)
        {
            _playerRootMovableRange = new Vector2(-1, 1) * (tapZoneView.Collider2D.size.x / 2 - 1);
        }

        UniTask IMovable.MoveToPointAsync(Vector3 point, CancellationToken token)
        {
            if (!IsActive) return UniTask.CompletedTask;

            MoveHorizontalAsync(point.x, token).Forget();

            return UniTask.CompletedTask;
        }

        // review: не понятно зачем нужен MovementController, если вьюшка сама может двигаться
        // review: получается контроллер не контроллер, а презентер некий.
        // review: Это к тому, что обработка логики должна быть вынесена в контроллер, а вьшка может только
        // review: поворачиваться, двигаться, скейльться и т.п. При том можно оставить инверсию зависимости.
        // review: Однако код управления должен находиться в одном классе
        void IMovable.Move(float horizontalDirection)
        {
            if (!IsActive) return;

            var translation = horizontalDirection * _movingSpeed;

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

        void IMovable.StopMoving()
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

        // review: эту часть можно вынести в модель/фабрику и т.п.
        void ISavableMono.Save()
        {
            DataSerializer.Save(Constants.PlayerPosition, transform.position.x);
        }

        void ISavableMono.Load()
        {
            if (DataSerializer.TryLoad<float>(Constants.PlayerPosition, out var x))
            {
                var transformPosition = transform.position;
                transformPosition.x = x;
                transform.position = transformPosition;
            }
        }
    }
}