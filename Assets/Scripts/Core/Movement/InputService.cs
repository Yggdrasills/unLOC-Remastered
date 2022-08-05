using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Core.Movement
{
    // review: объединена модель и контроллер. Должно быть следующее: InputController или MovementInputController
    // review: и модель для инпута движения
    // review: в идеале так: один контроллер для инпута в игре и много моделей разных инпутов,
    // review: которые данный контроллер обрабатывает
    // review: MonoBehaviour Update нужно заменить на Tick (либо кастомный и вызывать где-то еще,
    // review: либо от VContainer если говорим о контроллере)
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