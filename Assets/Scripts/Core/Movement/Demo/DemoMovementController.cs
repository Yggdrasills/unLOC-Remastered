using System;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class DemoMovementController : IStartable, IDisposable
    {
        private readonly TapZoneView _tapZoneView;
        private readonly IMovementService _movementService;
        private readonly DemoPlayerView _playerView;

        public DemoMovementController(
            TapZoneView tapZoneView,
            IMovementService movementService,
            DemoPlayerView playerView)
        {
            _tapZoneView = tapZoneView;
            _movementService = movementService;
            _playerView = playerView;
        }

        public void Start()
        {
            _playerView.IsActive = true;
            
            _tapZoneView.Clicked += MovePlayerToPoint;
        }

        public void Dispose()
        {
            _tapZoneView.Clicked -= MovePlayerToPoint;
        }

        private void MovePlayerToPoint(Vector3 position)
        {
            _movementService.StartMove(_playerView, position);
        }
    }
}