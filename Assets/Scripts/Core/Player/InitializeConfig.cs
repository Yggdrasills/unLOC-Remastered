using UnityEngine;

namespace SevenDays.unLOC.Core.Player
{
    [CreateAssetMenu(menuName = "SevenDays/unLoc/Core/Player/" + nameof(InitializeConfig), order = 0)]
    public class InitializeConfig : ScriptableObject
    {
        [SerializeField]
        private PlayerView _playerViewPrefab;

        [SerializeField]
        private CameraSettings _cameraSettingsPrefab;

        [SerializeField]
        private Vector3[] _cameraPath;

        [SerializeField]
        private Vector2 _playerSize;

        [SerializeField]
        private Vector2 _playerColliderSize;

        [SerializeField]
        private Vector2 _playerInitPosition;

        [SerializeField]
        private Vector2 _tapZoneSize;

        [SerializeField]
        private Vector3 _tapZonePosition;

        [SerializeField]
        private bool _disableTapZone;

        public PlayerView PlayerViewPrefab => _playerViewPrefab;

        public CameraSettings CameraSettingsPrefab => _cameraSettingsPrefab;

        public Vector3[] CameraPath => _cameraPath;

        public Vector2 PlayerSize => _playerSize;

        public Vector2 PlayerColliderSize => _playerColliderSize;

        public Vector2 TapZoneSize => _tapZoneSize;

        public Vector3 TapZonePosition => _tapZonePosition;

        public Vector2 PlayerInitPosition => _playerInitPosition;
        public bool DisableTapZone => _disableTapZone;
    }
}