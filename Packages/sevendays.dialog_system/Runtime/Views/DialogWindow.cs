using System;
using System.Collections;

using SevenDays.unLOC.Core;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogWindow : UIWindowBase, IPointerClickHandler
    {
        [field: SerializeField] public TextAsset DialogJson { get; set; }
        [field: SerializeField] public ChoiceView[] ChoiceViews;
        [SerializeField] private TextMeshProUGUI _textArea;

        private WaitForSeconds _revealInterval = new WaitForSeconds(0.01f);
        public event Action Clicked = delegate { };

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke();
        }

        public void ShowText(string text)
        {
            _textArea.text = text;

            StartCoroutine(RevealText());
        }

        public void Reset()
        {
            StopCoroutine(RevealText());

            if(ChoiceViews.Length is 0)
                return;
            
            foreach (var choiceView in ChoiceViews)
            {
                choiceView.Hide();
                choiceView.Button.onClick.RemoveAllListeners();
            }
        }

        private IEnumerator RevealText()
        {
            _textArea.maxVisibleCharacters = 0;
            yield return _revealInterval;
            
            var totalVisibleCharacters = _textArea.textInfo.characterCount;
            var counter = 0;

            while (true)
            {
                var visibleCount = counter % (totalVisibleCharacters + 1);
                _textArea.maxVisibleCharacters = visibleCount;
                
                if(visibleCount >= totalVisibleCharacters)
                    yield break;

                counter++;

                yield return _revealInterval;
            }

        }
    }
}