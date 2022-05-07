using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(SpriteRenderer),
        typeof(BoxCollider2D))]
    public class IconView : ClickableItem, IPointerEnterHandler, IPointerExitHandler
    {
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

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _iconRenderer.color = _highlightColor;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _iconRenderer.color = _defaultColor;
        }
    }
}