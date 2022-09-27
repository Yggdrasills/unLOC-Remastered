using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.InkWrapper.Views.Dialogs
{
    public class DialogView : MonoBehaviour, IDialogView
    {
        public event Action Clicked;

        bool IDialogView.IsRevealing => IsRevealing();

        [SerializeField]
        private TextMeshProUGUI _dialogText;

        [SerializeField, Tooltip("Text reveal interval in milliseconds")]
        private int _revealInterval = 10;

        [SerializeField, Tooltip("Between click interval in seconds")]
        private float _clickInterval = 0.1f;

        private int _totalVisibleCharacters;
        private float _lastClickTime;

        private CancellationTokenSource _cts;
        private CancellationToken _tokenOnDestroy;

        private void OnValidate()
        {
            if (_dialogText == null)
            {
                _dialogText = GetComponentInChildren<TextMeshProUGUI>();
            }

            Validated();
        }

        private void Awake()
        {
            _tokenOnDestroy = gameObject.GetCancellationTokenOnDestroy();

            Awakened();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!CanClick() || !IsTimerPassed())
            {
                return;
            }

            _lastClickTime = Time.time;

            if (IsRevealing())
            {
                RevealImmediately();
            }
            else
            {
                Clicked?.Invoke();
            }
        }

        async UniTaskVoid IDialogView.RevealAsync(string text)
        {
            _dialogText.text = text;

            using (_cts = CancellationTokenSource.CreateLinkedTokenSource(_tokenOnDestroy))
            {
                _dialogText.maxVisibleCharacters = 0;

                _totalVisibleCharacters = text.Length;

                var visibleCount = 0;

                for (int counter = 0; visibleCount < _totalVisibleCharacters; counter++)
                {
                    visibleCount = counter % (_totalVisibleCharacters + 1);
                    _dialogText.maxVisibleCharacters = visibleCount;

                    await UniTask.Delay(_revealInterval, cancellationToken: _cts.Token);
                }
            }

            _cts = null;
        }

        void IDialogView.Reveal()
        {
            RevealImmediately();
        }

        UniTask IDialogView.ShowAsync()
        {
            gameObject.SetActive(true);

            return UniTask.CompletedTask;
        }

        UniTask IDialogView.HideAsync()
        {
            gameObject.SetActive(false);

            return UniTask.CompletedTask;
        }

        protected virtual bool CanClick()
        {
            return true;
        }

        protected virtual void Awakened()
        {
        }

        protected virtual void Validated()
        {
        }

        private void RevealImmediately()
        {
            if (IsRevealing())
            {
                _cts.Cancel();
            }

            _dialogText.maxVisibleCharacters = _totalVisibleCharacters;
        }

        private bool IsRevealing()
        {
            return _cts is {IsCancellationRequested: false};
        }

        private bool IsTimerPassed()
        {
            if (Time.time > _lastClickTime + _clickInterval)
            {
                return true;
            }

            return false;
        }
    }
}