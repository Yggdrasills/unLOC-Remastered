using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;
using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Mechanoid;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests
{
    [RequireComponent(typeof(InteractableItem))]
    public class DoorBellQuest : QuestBase
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _bellClip;

        [SerializeField]
        private InteractableItem _interactableItem;

        [SerializeField]
        private WorkshopExitDoorView _doorView;

        [SerializeField]
        private MechanoidQuest _mechanoidQuest;

        [SerializeField]
        private TextAsset _dialogAsset;

        [SerializeField]
        private DialogView _dialogViewBehaviour;

        private DialogWrapper _wrapper;
        private IDialogView _dialogView;
        private IStorageRepository _storage;

        private bool _isBadEnd;

        private CancellationTokenSource _cts;

        [Inject, UsedImplicitly]
        private void Construct(LocalizationService localization,
            IStorageRepository storage)
        {
            _wrapper = new DialogWrapper(localization);
            _storage = storage;
        }

        private void Start()
        {
            if (_storage.IsExists(typeof(DoorBellQuest).FullName))
            {
                _interactableItem.Clicked += _doorView.LoadStreetStealth;
                return;
            }

            _dialogView = _dialogViewBehaviour;
            _cts = new CancellationTokenSource();

            if (_mechanoidQuest.IsCompleted)
            {
                RunAsync().Forget();
            }
            else
            {
                WaitForMechanoidQuestCompleted().Forget();
            }

            _interactableItem.Clicked += _doorView.LoadStreet;
        }

        private void OnDestroy()
        {
            _interactableItem.Clicked -= OnClick;
            _interactableItem.Clicked -= _doorView.LoadStreetStealth;
            _interactableItem.Clicked -= _doorView.LoadStreet;

            _cts?.Dispose();
        }

        private async UniTaskVoid WaitForMechanoidQuestCompleted()
        {
            await UniTask.WaitUntil(() => _mechanoidQuest.IsCompleted);

            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            _interactableItem.Clicked -= _doorView.LoadStreet;
            _interactableItem.Clicked += OnClick;

            while (!_cts.IsCancellationRequested)
            {
                await PlayAsync();
            }
        }

        private void OnClick()
        {
            _interactableItem.Clicked -= OnClick;

            _cts.Cancel();
            StartDialogAsync().Forget();
        }

        private async UniTask PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

            _audioSource.PlayOneShot(_bellClip);

            await UniTask.Delay(TimeSpan.FromSeconds(_bellClip.length));
        }

        private async UniTaskVoid StartDialogAsync()
        {
            await _dialogView.ShowAsync();

            var dialog = DialogWrapper.CreateDialog()
                .SetAction("end1", () => _isBadEnd = true)
                .SetAction("end2", () => _isBadEnd = false)
                .SetTextAsset(_dialogAsset)
                .OnComplete(() =>
                {
                    if (_isBadEnd)
                    {
                        _doorView.LoadCredits();
                    }
                    else
                    {
                        CompleteQuest();
                        _storage.Save(typeof(DoorBellQuest).FullName, true);

                        _dialogView.HideAsync().Forget();
                        _doorView.LoadStreetStealth();
                    }
                });

            _wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }
    }
}