using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Screwdriver
{
    public class ScrewdriverBitSwitcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private Nozzle _nozzle = Nozzle.None;

        [SerializeField]
        private ScrewdriverView _screwdriverView;

        [SerializeField]
        private SpriteRenderer[] _sprites;

        [SerializeField]
        private Color _targetColor = Color.green;

        private Color _initColor;

        private void Awake()
        {
            _initColor = _sprites[0].color;
        }

        private void OnValidate()
        {
            if (_sprites == null)
                _sprites = GetComponentsInChildren<SpriteRenderer>();

            if (_screwdriverView == null)
                _screwdriverView = GetComponentInParent<ScrewdriverView>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Colorize(_targetColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Colorize(_initColor);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _screwdriverView.ShowAsync(_nozzle).Forget();
        }

        private void Colorize(Color color)
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i].color = color;
            }
        }
    }
}