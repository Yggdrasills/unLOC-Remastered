using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Items
{
    public class ClickableItem : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked = delegate { };

        public bool CanInteract { protected get; set; } = true;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!CanInteract)
                return;

            Clicked.Invoke();
        }
    }
}