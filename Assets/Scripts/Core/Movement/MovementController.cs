using System;

using Cysharp.Threading.Tasks.Linq;

using SevenDays.unLOC.Core.Player;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public class MovementController : IDisposable
    {
        // review: нужно временно отключить тап зону
        private readonly TapZoneView _tapZoneView;
        private readonly IMovementService _movementService;
        private readonly IMovable _playerView;
        private readonly InputService _inputService;

        public MovementController(PlayerView playerView, IMovementService movementService, TapZoneView tapZoneView,
            InputService inputService)
        {
            _playerView = playerView;
            _movementService = movementService;
            _tapZoneView = tapZoneView;
            _inputService = inputService;
        }

        // review: класс не наследуется от IStartable. Если так и запланировано, то нужно вызывать его в конструкторе
        public void Start()
        {
            _tapZoneView.Clicked += OnClickedToTapZone;
            _playerView.IsActive = true;
            _inputService.HorizontalInput.WithoutCurrent().Subscribe(OnHorizontalInputChange);
            _playerView.StopMoving();
        }

        // review: диспоуз не вызывается
        void IDisposable.Dispose()
        {
            _tapZoneView.Clicked -= OnClickedToTapZone;
        }

        private void OnHorizontalInputChange(float direction)
        {
            if (direction == 0)
            {
                if (!_playerView.IsMoving && _inputService.PreviousInput != 0)
                {
                    _playerView.StopMoving();
                }

                return;
            }

            if (_playerView.IsMoving)
            {
                _playerView.StopMoving();
            }

            _playerView.Move(direction);
        }


        private void OnClickedToTapZone(Vector3 point)
        {
            var xClickPosition = point.x;

            _movementService.StartMove(_playerView, Vector3.right * xClickPosition);
        }
    }
}