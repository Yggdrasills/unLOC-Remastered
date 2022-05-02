using System;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Moving.Demo
{
    public class DemoMovingController : IStartable, IDisposable
    {
        private readonly TapZoneView _tapZoneView;
        private readonly IMovingService _movingService;
        private readonly DemoPlayerView _playerView;

        public DemoMovingController(
            TapZoneView tapZoneView,
            IMovingService movingService,
            DemoPlayerView playerView)
        {
            _tapZoneView = tapZoneView;
            _movingService = movingService;
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
            _movingService.StartMove(_playerView, position);
        }
    }
}