using System;

using DG.Tweening;

using SevenDays.unLOC.Core;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class
        InteractableItem : MonoBehaviour // , IPointerClickHandler // , IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Clicked = delegate { };

        [SerializeField]
        private IconView _iconView;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        private Tween _fadeTween;

        private bool _canClick;

        private void OnValidate()
        {
            if (_iconView == null)
            {
                _iconView = GetComponentInChildren<IconView>();
            }
        }

        private void Awake()
        {
            DoFade(0, 0);
        }

        private void OnEnable()
        {
            _iconView.Clicked += OnClick;
        }

        private void OnDisable()
        {
            _iconView.Clicked -= OnClick;

            ClearTween();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            DoFade(1, _fadeDuration);

            _canClick = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            DoFade(0, _fadeDuration);

            _canClick = false;
        }

        private void OnClick()
        {
            if (!_canClick)
                return;

            _canClick = false;

            DoFade(0, 0);

            Clicked.Invoke();
        }

        private void DoFade(float value, float duration)
        {
            ClearTween();

            _fadeTween = _iconView.Icon.DOFade(value, duration);
        }

        private void ClearTween()
        {
            if (_fadeTween != null && _fadeTween.IsActive())
                _fadeTween.Kill();
        }
    }
}