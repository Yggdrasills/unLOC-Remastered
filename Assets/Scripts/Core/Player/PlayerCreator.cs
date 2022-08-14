using System;

using Cinemachine;

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

        private IInitialize[] _initializes;
        private IDisposable[] _disposables;

        public PlayerCreator(InitializeConfig initializeConfig, Camera camera, IInputModel inputModel)
        {
            _initializeConfig = initializeConfig;
            _camera = camera;
            _inputModel = inputModel;
        }

        void IStartable.Start()
        {
            var player = CreatePlayer();

            CreateCamera(player);
            
            var tapZone = CreateTapZone();
            tapZone.enabled = !_initializeConfig.DisableTapZone;
            var playerMovement = PlayerMovement(tapZone, player);
            
            var playerAnimationController = new PlayerAnimationController(playerMovement, player);
            var movementController = new PlayerMovementController(tapZone, _inputModel, playerMovement, player.transform);

            _initializes = new IInitialize[] { playerAnimationController, movementController };
            _disposables = new IDisposable[] { playerAnimationController, movementController };

            foreach (var initialize in _initializes)
            {
                initialize.Initialize();
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

        private void CreateCamera(PlayerView player)
        {
            var cameraConfig = Object.Instantiate(_initializeConfig.CameraSettingsPrefab);
            var track = cameraConfig.GetComponentInChildren<CinemachineSmoothPath>();

            SetUpTrack(track);

            var vCam = cameraConfig.GetComponentInChildren<CinemachineVirtualCamera>();
            vCam.Follow = player.transform;
        }

        private PlayerView CreatePlayer()
        {
            var player = Object.Instantiate(_initializeConfig.PlayerViewPrefab);
            player.transform.position = _initializeConfig.PlayerInitPosition;

            player.CharacterScale.SetScale(_initializeConfig.PlayerSize, _initializeConfig.PlayerColliderSize);
            return player;
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