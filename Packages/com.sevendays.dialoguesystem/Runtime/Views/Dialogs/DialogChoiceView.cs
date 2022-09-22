using System.Collections.Generic;

using SevenDays.InkWrapper.Views.Choices;

using UnityEngine;

namespace SevenDays.InkWrapper.Views.Dialogs
{
    public class DialogChoiceView : DialogView, IDialogChoiceView
    {
        [SerializeField]
        private Transform _choiceContainer;

        [SerializeField]
        private ChoiceButtonView _choiceButtonPrefab;

        private List<GameObject> _choices;

        protected override void Awakened()
        {
            _choices = new List<GameObject>();
        }

        void IDialogChoiceView.RemoveChoices()
        {
            for (int i = 0, k = _choices.Count; i < k; i++)
            {
                Destroy(_choices[i]);
            }
        }

        IChoiceButtonView IDialogChoiceView.CreateChoice()
        {
            var choiceButton = Instantiate(_choiceButtonPrefab, _choiceContainer);

            _choices.Add(choiceButton.gameObject);

            return choiceButton;
        }
    }
}