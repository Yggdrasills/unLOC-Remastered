using System.Collections.Generic;

using Cysharp.Threading.Tasks;

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

        void IDialogChoiceView.HideChoices()
        {
            for (int i = 0, k = _choices.Count; i < k; i++)
            {
                Destroy(_choices[i]);
            }

            _choiceContainer.gameObject.SetActive(false);
        }

        public UniTask PrependShowAsync()
        {
            _choiceContainer.gameObject.SetActive(true);

            return UniTask.CompletedTask;
        }

        IChoiceButtonView IDialogChoiceView.GetChoice()
        {
            var choiceButton = Instantiate(_choiceButtonPrefab, _choiceContainer);

            _choices.Add(choiceButton.gameObject);

            return choiceButton;
        }
    }
}