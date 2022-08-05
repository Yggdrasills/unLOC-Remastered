using Cinemachine;

using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player.Animations;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Player
{
    public class PlayerCreator : IStartable
    {
        private readonly InitializeConfig _initializeConfig;
        private readonly IMovementService _movementService;
        private readonly Camera _camera;

        public PlayerCreator(InitializeConfig initializeConfig, IMovementService movementService, Camera camera)
        {
            _initializeConfig = initializeConfig;
            _movementService = movementService;
            _camera = camera;
        }

        void IStartable.Start()
        {
            var player = Object.Instantiate(_initializeConfig.PlayerViewPrefab);
            player.transform.position = _initializeConfig.PlayerInitPosition;

            player.CharacterScale.SetScale(_initializeConfig.PlayerSize, _initializeConfig.PlayerColliderSize);

            var cameraConfig = Object.Instantiate(_initializeConfig.CameraSettingsPrefab);
            // review: GetComponentInChildren не есть хорошо
            var track = cameraConfig.GetComponentInChildren<CinemachineSmoothPath>();

            SetUpTrack(track);

            var vCam = cameraConfig.GetComponentInChildren<CinemachineVirtualCamera>();
            vCam.Follow = player.transform;

            var input = player.gameObject.AddComponent<InputService>();
            
            var tapZone = new GameObject("TapZone").AddComponent<TapZoneView>();
            tapZone.SetUp(_camera,_initializeConfig.TapZoneSize,_initializeConfig.TapZonePosition);
            player.SetRange(tapZone);

            var playerAnimationController = new PlayerAnimationController(player);
            playerAnimationController.Start();
            
            var movementController = new MovementController(player, _movementService, tapZone, input);
            movementController.Start();
        }

        private void SetUpTrack(CinemachineSmoothPath track)
        {
            for (var i = 0; i < _initializeConfig.CameraPath.Length; i++)
            {
                track.m_Waypoints[i].position = _initializeConfig.CameraPath[i];
            }
        }
    }
}