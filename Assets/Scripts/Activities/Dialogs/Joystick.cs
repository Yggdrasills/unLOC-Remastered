using System;

using UnityEngine;

namespace Activities.Dialogs
{
    public class Joystick : MonoBehaviour
    {
        public event Action<Vector2> Moving;

        [SerializeField]
        private int _dragMovementDistance = 30;

        [SerializeField]
        private int _dragOffsetDistance = 100;

        [SerializeField]
        private RectTransform _target;

        [SerializeField]
        private Camera _camera;

        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _target,
                Input.mousePosition,
                _camera,
                out var offset);

            offset = Vector2.ClampMagnitude(offset, _dragOffsetDistance) / _dragOffsetDistance;

            _target.anchoredPosition = offset * _dragMovementDistance;

            Moving?.Invoke(offset);
        }
    }
}