using System;

using SevenDays.unLOC.Core;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogWindow : UIWindowBase, IPointerClickHandler
    {
        [field: SerializeField] public TextAsset DialogJson { get; private set; }
        [field: SerializeField] public ChoiceView[] ChoiceViews;
        [SerializeField] private TextMeshProUGUI _textArea;

        public event Action Clicked = delegate { };
        
        public void SetText(string text)
        {
            _textArea.text = text;
        }

        public void Reset()
        {
            foreach (var choiceView in ChoiceViews)
            {
                choiceView.Hide();
                choiceView.Button.onClick.RemoveAllListeners();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke();
        }
    }
}