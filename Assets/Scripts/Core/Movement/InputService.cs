using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    public class InputService : MonoBehaviour
    {
        public AsyncReactiveProperty<float> HorizontalInput { get; } = new AsyncReactiveProperty<float>(0);
        public float PreviousInput { get; private set; }

        private void Update()
        {
            HorizontalInput.Value = Input.GetAxis("Horizontal");
            PreviousInput = HorizontalInput.Value;
        }
    }
}