using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Core.Movement
{
    public class TapZoneView : MonoBehaviour, IPointerClickHandler
    {
        public Action<Vector3> Clicked = delegate { };

        public BoxCollider2D Collider2D { get; private set; }

        private void OnValidate()
        {
            Collider2D = GetComponent<BoxCollider2D>();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (Camera.main == null) return;

            var position = Camera.main.ScreenToWorldPoint(eventData.position);

            Clicked.Invoke(position);
        }
    }
}