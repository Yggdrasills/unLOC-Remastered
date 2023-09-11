using System;

using SevenDays.InkWrapper.Views.Choices;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Activities.Dialogs
{
    public class ClickableChoiceView : MonoBehaviour, IChoiceButtonView, IPointerClickHandler
    {
        [SerializeField]
        private TextMeshProUGUI _btnText;

        [SerializeField]
        private AppearanceStrategy _appearanceStrategy;

        private Action _clicked;

        private void OnValidate()
        {
            if (_btnText == null)
            {
                _btnText = GetComponentInChildren<TextMeshProUGUI>(true);
            }

            if (_appearanceStrategy == null)
            {
                _appearanceStrategy = gameObject.AddComponent<DefaultAppearanceStrategy>();
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            _clicked?.Invoke();
        }

        void IChoiceButtonView.SetClickAction(Action action)
        {
            _clicked = action;
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