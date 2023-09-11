using System;

using Cysharp.Threading.Tasks;

namespace SevenDays.unLOC.Core.Movement
{
    public class HorizontalInputModel : IInputModel
    {
        public AsyncReactiveProperty<float> Input { get; } = new AsyncReactiveProperty<float>(0);
        public float PreviousInput { get;  set; }

        public Func<float> ValueGetter { get; }

        public HorizontalInputModel()
        {
            ValueGetter = () => UnityEngine.Input.GetAxis("Horizontal");
        }
    }
}