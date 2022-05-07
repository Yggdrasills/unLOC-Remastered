using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class TapZoneView : MonoBehaviour, IPointerClickHandler
    {
        public Action<Vector3> Clicked = delegate { };

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (Camera.main == null) return;

            var position = Camera.main.ScreenToWorldPoint(eventData.position);

            Clicked.Invoke(position);
        }
    }
}