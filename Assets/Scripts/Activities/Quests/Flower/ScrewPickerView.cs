using System;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine;
using UnityEngine.EventSystems;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class ScrewPickerView : PickableBase, IPointerClickHandler
    {
        [SerializeField]
        private InventoryItem _inventoryItem = InventoryItem.ScrewEdge3;

        public override InventoryItem Type => _inventoryItem;

        [SerializeField, Tooltip("Item pick max amount")]
        private int _maxPickAmount = 4;

        private IInventoryService _inventory;

        private int _currentClickAmount;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _currentClickAmount++;

            _inventory.AddAsync(this).Forget();
        }

        public override UniTask VisualisePickAsync()
        {
            if (_currentClickAmount >= _maxPickAmount)
            {
                gameObject.SetActive(false);
            }

            return UniTask.CompletedTask;
        }
    }
}