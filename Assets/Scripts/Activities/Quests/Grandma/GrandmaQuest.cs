using System;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.DialogSystem.Runtime;
using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Grandma.Visualization;
using SevenDays.unLOC.Inventory.Services;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Grandma
{
    public class GrandmaQuest : QuestBase
    {
        [SerializeField]
        private GlassesView _glassesPrefab;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private DraggableItemView[] _draggables;

        [SerializeField]
        private InteractableItem _grandmaView;

        private int _droppedAmount;

        private IInventoryService _inventory;
        private DialogService _dialogService;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory, DialogService dialogService)
        {
            _inventory = inventory;
            _dialogService = dialogService;
        }

        public void SetDialogTag()
        {
            _dialogService.SubscribeTagAction(DialogTag.GrandmaQuestStart, StartQuest);
        }

        private void StartQuest()
        {
            _camera.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            for (int i = 0; i < _draggables.Length; i++)
            {
                _draggables[i].Dropped += OnDropped;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _draggables.Length; i++)
            {
                _draggables[i].Dropped -= OnDropped;
            }
        }

        private void OnDropped()
        {
            _droppedAmount++;

            if (_droppedAmount >= _draggables.Length)
            {
                _grandmaView.Disable();

                gameObject.SetActive(false);
                _camera.gameObject.SetActive(false);

                var glasses = Instantiate(_glassesPrefab);
                _inventory.AddAsync(glasses).Forget();

                // todo: start dialogue
                CompleteQuest();
            }
        }
    }
}