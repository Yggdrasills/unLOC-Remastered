using SevenDays.unLOC.Core;

using TMPro;

using UnityEngine;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogWindow : UIWindowBase
    {
        [field: SerializeField] public TextAsset DialogJson { get; private set; }
        [field: SerializeField] public ChoiceView[] ChoiceViews;
        [SerializeField] private TextMeshProUGUI _textArea;

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
    }
}