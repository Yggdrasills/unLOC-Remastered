using Cinemachine;

using UnityEngine;

namespace SevenDays.unLOC.Core.Player
{
    public class CameraSettings : MonoBehaviour
    {
        [field: SerializeField]
        public CinemachineVirtualCamera VCam { get; private set; }

        [field: SerializeField]
        public CinemachineSmoothPath Track { get; private set; }
    }
}