using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SevenDays.unLOC.Activities.Quests.Flower.Screwdriver
{
    public class ScrewdriverBitSwitcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private Nozzle _nozzle = Nozzle.None;

        [SerializeField]
        private ScrewdriverView _screwdriverView;

        [SerializeField]
        private Image[] _images;

        [SerializeField]
        private Color _targetColor = Color.green;

        private Color _initColor;

        private void Awake()
        {
            _initColor = _images[0].color;
        }

        private void OnValidate()
        {
            if (_images == null)
                _images = GetComponentsInChildren<Image>();

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
            _screwdriverView.ShowAsync(_nozzle);
        }

        private void Colorize(Color color)
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].color = color;
            }
        }
    }
}