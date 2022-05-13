using System;
using System.Collections;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogWindow : UIWindowBase, IPointerClickHandler
    {
        [SerializeField]
        private UnityEvent _dialogueEnded;

        [field: SerializeField]
        public TextAsset DialogJson { get; set; }

        [field: SerializeField]
        public ChoiceView[] ChoiceViews;

        [SerializeField]
        private TextMeshProUGUI _textArea;

        private WaitForSeconds _revealInterval = new WaitForSeconds(0.01f);
        public event Action Clicked = delegate { };

        private UniTaskCompletionSource _revealCompletionSource;

        private int _totalVisibleCharacters;

        private Coroutine _revealRoutine;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke();
        }

        public Action GetDialogueEndAction()
        {
            return delegate { _dialogueEnded?.Invoke(); };
        }

        public async UniTask ShowTextAsync(string text)
        {
            _revealCompletionSource = new UniTaskCompletionSource();

            _textArea.text = text;
            

            _revealRoutine = StartCoroutine(RevealText());

            await _revealCompletionSource.Task;
        }

        public void RevealImmediately()
        {
            StopRevealing();

            _textArea.maxVisibleCharacters = _totalVisibleCharacters;
            _revealCompletionSource.TrySetCanceled();
        }

        public void ResetToDefault()
        {
            StopRevealing();

            if (ChoiceViews.Length is 0)
                return;

            foreach (var choiceView in ChoiceViews)
            {
                choiceView.Hide();
                choiceView.Button.onClick.RemoveAllListeners();
            }
        }

        private void StopRevealing()
        {
            if (_revealRoutine != null)
            {
                StopCoroutine(_revealRoutine);
                _revealRoutine = null;
            }
        }

        private IEnumerator RevealText()
        {
            _textArea.maxVisibleCharacters = 0;
            yield return _revealInterval;
            
            _totalVisibleCharacters = _textArea.textInfo.characterCount;

            var visibleCount = 0;

            for (int counter = 0; visibleCount < _totalVisibleCharacters; counter++)
            {
                visibleCount = counter % (_totalVisibleCharacters + 1);
                _textArea.maxVisibleCharacters = visibleCount;

                yield return _revealInterval;
            }

            _revealCompletionSource.TrySetResult();
        }
    }
}