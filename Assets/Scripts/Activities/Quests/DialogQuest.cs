using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;
using SevenDays.unLOC.Storage;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests
{
    public class DialogQuest : QuestBase
    {
        protected IStorageRepository Storage;

        [SerializeField]
        private DialogView _dialogViewBehaviour;

        [SerializeField]
        private TextAsset _dialogAsset;

        private IDialogView _dialogView;
        private DialogWrapper _wrapper;

        public void Setup(LocalizationService localization,
            IStorageRepository storage)
        {
            _wrapper = new DialogWrapper(localization);
            Storage = storage;
        }

        public void Initialize()
        {
            _dialogView = _dialogViewBehaviour;

            Initialized();
        }

        protected void StartDialog()
        {
            _dialogView.ShowAsync().Forget();
            StartDialogAsync().Forget();
        }

        protected virtual void Initialized()
        {
        }

        protected virtual Dictionary<string, Action> GetDialogTagActions()
        {
            return null;
        }

        protected virtual void DialogCompleted()
        {
        }

        private async UniTaskVoid StartDialogAsync()
        {
            await _dialogView.ShowAsync();

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .OnComplete(() =>
                {
                    Storage.Save(GetType().FullName, true);
                    _dialogView.HideAsync().Forget();

                    CompleteQuest();

                    DialogCompleted();
                });

            var keyActions = GetDialogTagActions();

            if (keyActions != null)
            {
                foreach (var item in GetDialogTagActions())
                {
                    dialog.SetAction(item.Key, item.Value);
                }
            }

            _wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }
    }
}