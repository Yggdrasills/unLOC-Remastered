using System;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public class PlayerMovementController : IInitialize, IDisposable
    {
        private readonly TapZoneView _tapZoneView;
        private readonly IInputModel _inputModel;
        private readonly Transform _playerTransform;
        private readonly MovementModel _movementModel;

        public PlayerMovementController(TapZoneView tapZoneView, IInputModel inputModel, MovementModel movementModel, Transform playerTransform)
        {
            _tapZoneView = tapZoneView;
            _inputModel = inputModel;
            _movementModel = movementModel;
            _playerTransform = playerTransform;
        }

        public void Initialize()
        {
            _tapZoneView.Clicked += OnClickedToTapZone;
            _inputModel.Input.WithoutCurrent().Subscribe(OnHorizontalInputChange);
            StopMoveToPoint();
            _movementModel.OnStop();
        }

        private void Move(float horizontalNormal)
        {
            var translation = horizontalNormal * _movementModel.MovingSpeed;

            if (_playerTransform.position.x >= _movementModel.Range.y && translation > 0 ||
                _playerTransform.position.x <= _movementModel.Range.x && translation < 0)
            {
                _movementModel.IsMoving = false;
                _movementModel.OnStop();
                return;
            }

            RotatePlayer(translation, 0);

            translation *= Time.deltaTime;

            _playerTransform.Translate(translation, 0, 0);

            _movementModel.IsMoving = true;
            _movementModel.OnMove();
        }


        private void RotatePlayer(float horizontalPoint, float comparableValue)
        {
            var rotationValue = horizontalPoint < comparableValue ? MovementModel.LeftSideValue :MovementModel.RightSideValue;
            if (Math.Abs(_playerTransform.localScale.x - rotationValue) > .0f)
                _playerTransform.localScale = new Vector3(rotationValue, _playerTransform.localScale.y, 1);
        }

        private void OnHorizontalInputChange(float normal)
        {
            if (normal == 0)
            {
                if (_inputModel.PreviousInput == 0 || _movementModel.IsMovingToPoint)
                {
                    return;
                }

                _movementModel.IsMoving = false;
                StopMoveToPoint();
                return;
            }
            
            if (_movementModel.IsMovingToPoint)
            {
                if (_inputModel.PreviousInput == 0)
                {
                    StopMoveToPoint();
                }
                else if(_movementModel.IsMoving)
                {
                    return;
                }
            }
            Move(normal);
        }


        private void OnClickedToTapZone(Vector3 point)
        {
            StopMoveToPoint();
            _movementModel.OnStop();
            _movementModel.IsMovingToPoint = false;
            MoveToPointAsync(Vector3.right * point.x).Forget();
        }


        private async UniTask MoveToPointAsync(Vector3 point)
        {
            _movementModel.UpdateToken();
            await MoveHorizontalAsync(point.x, _movementModel.MovingToken.Token);
        }

        private void StopMoveToPoint()
        {
            if (_movementModel.ActiveTween is { active: true })
            {
                _movementModel.ActiveTween.Kill();
            }
            _movementModel.IsMovingToPoint = false;
            _movementModel.OnStop();
        }

        private async UniTask MoveHorizontalAsync(float horizontalPoint, CancellationToken token)
        {
            var viewXPosition = _playerTransform.transform.position.x;

            RotatePlayer(horizontalPoint, viewXPosition);

            var distance = Mathf.Abs(horizontalPoint - viewXPosition);

            _movementModel.IsMovingToPoint = true;
            _movementModel.OnMove();
            
            _movementModel.ActiveTween = _playerTransform.transform.DOMoveX(horizontalPoint, distance / _movementModel.MovingSpeed).SetEase(Ease.Linear);

            await _movementModel.ActiveTween.AwaitForComplete(cancellationToken: token);
            
            _movementModel.IsMovingToPoint = false;
            _movementModel.OnStop();
        }

        public void Dispose()
        {
            _tapZoneView.Clicked -= OnClickedToTapZone;
        }
    }
}