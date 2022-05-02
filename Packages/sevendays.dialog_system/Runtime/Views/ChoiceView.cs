using SevenDays.unLOC.Core;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.DialogSystem.Runtime
{
    public class ChoiceView : UIWindowBase
    {
        [SerializeField] private TextMeshProUGUI _textArea;
        public Button Button;

        public void SetText(string text)
        {
            _textArea.text = text;
        }
    }
}