using System;

using Cinemachine;

using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player.Animations;

using UnityEngine;

using VContainer.Unity;

using Object = UnityEngine.Object;

namespace SevenDays.unLOC.Core.Player
{
    public class PlayerCreator : IStartable, IDisposable
    {
        private readonly InitializeConfig _initializeConfig;
        private readonly Camera _camera;
        private readonly IInputModel _inputModel;
        private readonly PlayerView _player;

        private IInstallable[] _initializes;
        private IDisposable[] _disposables;

        public PlayerCreator(InitializeConfig initializeConfig, Camera camera, IInputModel inputModel, PlayerView player)
        {
            _initializeConfig = initializeConfig;
            _camera = camera;
            _inputModel = inputModel;
            _player = player;
        }

        void IStartable.Start()
        {

            CreateCamera(_player.transform);

            var tapZone = CreateTapZone();
            tapZone.enabled = !_initializeConfig.DisableTapZone;
            var playerMovement = PlayerMovement(tapZone, _player);

            var playerAnimationController = new PlayerAnimationController(playerMovement, _player);
            var movementController =
                new PlayerMovementController(tapZone, _inputModel, playerMovement, _player);

            _initializes = new IInstallable[] { playerAnimationController, movementController };
            _disposables = new IDisposable[] { playerAnimationController, movementController };

            foreach (var initialize in _initializes)
            {
                initialize.Install();
            }
        }

        private MovementModel PlayerMovement(TapZoneView tapZone, PlayerView player)
        {
            var playerMoveRange = new Vector2(-1, 1) * (tapZone.Collider2D.size.x / 2 - 1);
            var model = new MovementModel(playerMoveRange, player.MovingSpeed);
            return model;
        }

        private TapZoneView CreateTapZone()
        {
            var tapZone = new GameObject(nameof(TapZoneView)).AddComponent<TapZoneView>();
            tapZone.SetUp(_camera, _initializeConfig.TapZoneSize, _initializeConfig.TapZonePosition);
            return tapZone;
        }

        private void CreateCamera(Transform player)
        {
            var cameraConfig = Object.Instantiate(_initializeConfig.CameraSettingsPrefab);
            SetUpTrack(cameraConfig.Track);
            cameraConfig.VCam.Follow = player;
        }

        private void SetUpTrack(CinemachineSmoothPath track)
        {
            for (var i = 0; i < _initializeConfig.CameraPath.Length; i++)
            {
                track.m_Waypoints[i].position = _initializeConfig.CameraPath[i];
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}