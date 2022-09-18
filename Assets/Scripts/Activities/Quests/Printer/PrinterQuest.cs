using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Printer
{
    public class PrinterQuest : QuestBase
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private WiresView _wiresPrefab;

        [SerializeField]
        private InteractableItem _printerView;

        [SerializeField]
        private ClickableItem _clickableItem;

        [SerializeField]
        private int _bottlesAmount = 5;

        private IInventoryService _inventory;
        private DataStorage _storage;

        private int _droppedBottlesAmount;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory,
            DataStorage storage)
        {
            _inventory = inventory;
            _storage = storage;
        }

        private void Awake()
        {
            if (_storage.TryLoad(typeof(PrinterQuest).FullName, out _droppedBottlesAmount))
            {
                if (_droppedBottlesAmount >= _bottlesAmount)
                {
                    Complete();
                }
            }

            _clickableItem.Clicked += OnClick;
        }

        private void OnDestroy()
        {
            _clickableItem.Clicked -= OnClick;

            _storage.Save(typeof(PrinterQuest).FullName, _droppedBottlesAmount);
        }

        public void OnClick()
        {
            if (!_inventory.Contains(InventoryItem.Bottle))
                return;

            _inventory.Use(InventoryItem.Bottle);

            _droppedBottlesAmount++;

            if (_droppedBottlesAmount < _bottlesAmount)
                return;

            GiveWires();
            Complete();
        }

        private void Complete()
        {
            _printerView.Disable();
            _content.SetActive(false);

            CompleteQuest();
        }

        private void GiveWires()
        {
            var wires = Instantiate(_wiresPrefab);

            _inventory.AddAsync(wires).Forget();
        }
    }
}