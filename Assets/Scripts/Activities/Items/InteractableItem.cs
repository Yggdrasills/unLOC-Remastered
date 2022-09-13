using System;

using DG.Tweening;

using SevenDays.unLOC.Core.Player;

using UnityEngine;
using UnityEngine.Events;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableItem : MonoBehaviour
    {
        public event Action Clicked = delegate { };

        [SerializeField]
        private UnityEvent _clickedUnityEvent;

        [SerializeField]
        private IconView _iconView;

        [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        private Tween _fadeTween;

        private bool _canClick;

        private void OnValidate()
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }

            if (_iconView == null)
            {
                _iconView = GetComponentInChildren<IconView>();
            }

            Validated();
        }

        private void Awake()
        {
            DoFade(0, 0);
        }

        private void OnEnable()
        {
            _iconView.Clicked += OnClick;

            Enabled();
        }

        private void OnDisable()
        {
            _iconView.Clicked -= OnClick;

            ClearTween();
            Disabled();
        }

        public void Disable()
        {
            _iconView.gameObject.SetActive(false);
            _collider.enabled = false;
            enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            ToggleEntry(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.GetComponent<PlayerTag>())
                return;

            ToggleEntry(false);
        }

        private void OnClick()
        {
            if (!_canClick)
                return;

            _canClick = false;

            ToggleEntry(false);

            Clicked.Invoke();
            _clickedUnityEvent.Invoke();
        }

        private void ToggleEntry(bool isEnter)
        {
            DoFade(isEnter ? 1 : 0, _fadeDuration);

            _canClick = isEnter;
            _iconView.CanInteract = isEnter;
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

        protected virtual void Enabled()
        {
        }

        protected virtual void Disabled()
        {
        }

        protected virtual void Validated()
        {
        }
    }
}