using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Quests.Flower.Screwdriver
{
    public class ScrewdriverPowerButtonView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private float _duration = 0.5f;

        private Tween _scaleTween;

        private bool _isShown;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            DoScale();
        }

        private void DoScale()
        {
            if (_scaleTween != null && _scaleTween.IsActive())
                _scaleTween.Kill();

            var scale = _isShown ? Vector3.zero : Vector3.one;

            _scaleTween = _target.DOScale(scale, _duration);

            _isShown = !_isShown;
        }
    }
}