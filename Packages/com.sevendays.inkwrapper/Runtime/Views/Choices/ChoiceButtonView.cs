using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.InkWrapper.Views.Choices
{
    public class ChoiceButtonView : MonoBehaviour, IChoiceButtonView
    {
        [SerializeField]
        private TextMeshProUGUI _btnText;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private AppearanceStrategy _appearanceStrategy;

        private void OnValidate()
        {
            if (_btnText == null)
            {
                _btnText = GetComponentInChildren<TextMeshProUGUI>(true);
            }

            if (_button == null)
            {
                _button = GetComponentInChildren<Button>(true);
            }

            if (_appearanceStrategy == null)
            {
                _appearanceStrategy = gameObject.AddComponent<DefaultAppearanceStrategy>();
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        void IChoiceButtonView.SetClickAction(Action action)
        {
            _button.onClick.AddListener(() => action?.Invoke());
        }

        void IChoiceButtonView.SetText(string text)
        {
            _btnText.text = text;
        }

        void IChoiceButtonView.Show()
        {
            _appearanceStrategy.Show();
        }
    }
}