using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Views.Choices;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.unLOC.Utils.IndexGenerators;

using UnityEngine;

namespace Activities.Dialogs
{
    public class RadialChoiceDialogView : DialogView, IDialogChoiceView
    {
        [SerializeField]
        private GameObject _choiceContainer;

        [SerializeField]
        private ClickableChoiceView[] _choices;

        private IIndexGenerator _indexGenerator;

        protected override void Awakened()
        {
            _indexGenerator = new SequenceIndexGenerator(_choices.Length);

            DisableChoices();
        }

        void IDialogChoiceView.HideChoices()
        {
            DisableChoices();
        }

        UniTask IDialogChoiceView.PrependShowAsync()
        {
            _indexGenerator.Reset();
            _choiceContainer.SetActive(true);

            return UniTask.CompletedTask;
        }

        IChoiceButtonView IDialogChoiceView.GetChoice()
        {
            var index = _indexGenerator.Get();

            return _choices[index];
        }

        private void DisableChoices()
        {
            for (int i = 0; i < _choices.Length; i++)
            {
                _choices[i].gameObject.SetActive(false);
            }

            _choiceContainer.SetActive(false);
        }
    }
}