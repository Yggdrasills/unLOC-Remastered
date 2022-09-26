using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Grandma
{
    public class GrandmaView : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _dialogAsset;

        [SerializeField]
        private DialogView _dialogViewBehaviour;

        [SerializeField]
        private GrandmaQuest _quest;

        private DialogWrapper _wrapper;
        private IDialogView _dialogView;

        [Inject, UsedImplicitly]
        private void Construct(LocalizationService localization)
        {
            _wrapper = new DialogWrapper(localization);
        }

        private void Awake()
        {
            _dialogView = _dialogViewBehaviour;
        }

        public void StartDialog()
        {
            StartDialogAsync().Forget();
        }

        private async UniTaskVoid StartDialogAsync()
        {
            await _dialogView.ShowAsync();

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .SetAction("grandma_quest_start", StartQuest)
                .OnComplete(() => _dialogView.HideAsync().Forget());

            _wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }

        private void StartQuest()
        {
            _quest.StartQuest();
            _dialogView.HideAsync().Forget();
        }
    }
}