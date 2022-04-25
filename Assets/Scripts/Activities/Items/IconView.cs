using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(SpriteRenderer),
        typeof(BoxCollider2D))]
    public class IconView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Clicked = delegate { };

        public SpriteRenderer Icon => _iconRenderer;

        [SerializeField]
        private SpriteRenderer _iconRenderer;

        [SerializeField]
        private Color _highlightColor = Color.green;

        [SerializeField]
        private Color _defaultColor = Color.white;

        private void OnValidate()
        {
            if (_iconRenderer == null)
            {
                _iconRenderer = GetComponent<SpriteRenderer>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _iconRenderer.color = _highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _iconRenderer.color = _defaultColor;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("tt2");
            Clicked?.Invoke();
        }
    }
}