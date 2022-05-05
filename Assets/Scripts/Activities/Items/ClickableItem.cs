using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Items
{
    public class ClickableItem : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked = delegate { };

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke();
        }
    }
}