using System;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    [RequireComponent(typeof(ClickableItem))]
    public class ScrewdriverPickableView : PickableBase
    {
        [SerializeField]
        private ClickableItem _clickableItem;

        [SerializeField]
        private GameObject _screwdriverContent;

        private IInventoryService _inventory;

        private Action _inventoryAddAction;

        private void OnValidate()
        {
            if (_clickableItem == null)
                _clickableItem = GetComponent<ClickableItem>();
        }

        [Inject]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        private void Awake()
        {
            _inventoryAddAction = () => _inventory.AddAsync(this).Forget();
        }

        private void OnEnable()
        {
            _clickableItem.Clicked += _inventoryAddAction;
        }

        private void OnDisable()
        {
            _clickableItem.Clicked -= _inventoryAddAction;
        }

        public override InventoryItem Type => InventoryItem.Screwdriver;

        public override Action GetInventoryClickStrategy()
        {
            return () => _screwdriverContent.gameObject.SetActive(true);
        }
    }
}