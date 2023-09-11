using System;

using Cysharp.Threading.Tasks;

namespace SevenDays.unLOC.Core.Movement
{
    public interface IInputModel
    {
        AsyncReactiveProperty<float> Input { get; }
        float PreviousInput { get; set; }
        Func<float> ValueGetter { get; }

    }
}