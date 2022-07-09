using Cysharp.Threading.Tasks;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement
{
    public class InputService : ITickable
    {
        public AsyncReactiveProperty<float> HorizontalInput { get; } = new AsyncReactiveProperty<float>(0);
        public float LastDirection { get; private set; }
        void ITickable.Tick()
        {
            HorizontalInput.Value = Input.GetAxis("Horizontal");
            LastDirection = HorizontalInput.Value;
        }
    }
}