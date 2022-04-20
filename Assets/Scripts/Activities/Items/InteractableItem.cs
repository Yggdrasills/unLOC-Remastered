using System;

using DG.Tweening;

using SevenDays.unLOC.Core;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableItem : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked = delegate { };

        [SerializeField]
        private SpriteRenderer _iconRenderer;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        private Tween _fadeTween;

        private bool _canClick;

        private void Awake()
        {
            DoFade(0, 0);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!_canClick)
                return;

            _canClick = false;

            DoFade(0, 0);

            Clicked.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            DoFade(1, _fadeDuration);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            DoFade(0, _fadeDuration);

            _canClick = true;
        }

        private void DoFade(float value, float duration)
        {
            if (_fadeTween != null && _fadeTween.IsActive())
                _fadeTween.Kill();

            _fadeTween = _iconRenderer.DOFade(value, duration);
        }
    }
}