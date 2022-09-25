using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;
using SevenDays.unLOC.Activities.Workshop.Views;
using SevenDays.unLOC.Storage;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Activities.Quests.Manager
{
    public class ManagerDialogQuest : QuestBase
    {
        [SerializeField]
        private Button _managerButton;

        [SerializeField]
        private DialogView _dialogViewBehaviour;

        [SerializeField]
        private TextAsset _dialogAsset;

        private IStorageRepository _storage;
        private IDialogView _dialogView;
        private DialogWrapper _wrapper;

        public void Setup(LocalizationService localization,
            IStorageRepository storage)
        {
            _wrapper = new DialogWrapper(localization);
            _storage = storage;
        }

        public void Initialize()
        {
            _dialogView = _dialogViewBehaviour;

            if (_storage.IsExists(nameof(ManagerDialogQuest)))
            {
                _managerButton.gameObject.SetActive(false);
                gameObject.SetActive(false);

                return;
            }

            _managerButton.onClick.AddListener(StartDialog);
        }

        private void StartDialog()
        {
            if (!_storage.IsExists(typeof(NoteView).FullName))
            {
                return;
            }

            _managerButton.gameObject.SetActive(false);
            _dialogView.ShowAsync().Forget();
            StartDialogAsync().Forget();
        }

        private async UniTaskVoid StartDialogAsync()
        {
            await _dialogView.ShowAsync();

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .OnComplete(() =>
                {
                    _storage.Save(nameof(ManagerDialogQuest), true);
                    _dialogView.HideAsync().Forget();

                    CompleteQuest();
                });

            _wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }
    }
}