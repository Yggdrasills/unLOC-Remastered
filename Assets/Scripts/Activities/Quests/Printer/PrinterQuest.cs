using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

using UnityEngine;
using UnityEngine.EventSystems;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Printer
{
    public class PrinterQuest : QuestBase, IPointerClickHandler
    {
        [SerializeField]
        private WiresView _wiresPrefab;

        [SerializeField]
        private InteractableItem _printerView;

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private int _bottlesAmount = 5;

        private IInventoryService _inventory;

        private int _droppedBottlesAmount;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_inventory.Contains(InventoryItem.Bottle))
                return;

            _inventory.Use(InventoryItem.Bottle);

            _droppedBottlesAmount++;

            if (_droppedBottlesAmount < _bottlesAmount)
                return;

            var wires = Instantiate(_wiresPrefab);

            _inventory.AddAsync(wires);

            _printerView.Disable();

            _container.SetActive(false);

            CompleteQuest();
        }
    }
}