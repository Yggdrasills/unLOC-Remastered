using System;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    [RequireComponent(typeof(ClickableItem))]
    public class ScrewdriverPickableView : PickableBase
    {
        public override InventoryItem Type => InventoryItem.Screwdriver;

        [SerializeField]
        private ClickableItem _clickableItem;

        private IInventoryService _inventory;

        private DataStorage _storage;

        private Action _inventoryAddAction;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory, DataStorage storage)
        {
            _inventory = inventory;
            _storage = storage;
        }

        private void OnValidate()
        {
            if (_clickableItem == null)
                _clickableItem = GetComponent<ClickableItem>();
        }

        private void Awake()
        {
            if (_storage.IsExists(typeof(ScrewdriverPickableView).FullName))
            {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _inventoryAddAction = () =>
            {
                _inventory.AddAsync(this).Forget();

                gameObject.SetActive(false);
                _storage.Save(typeof(ScrewdriverPickableView).FullName, true);
            };

            _clickableItem.Clicked += _inventoryAddAction;
        }

        private void OnDestroy()
        {
            _clickableItem.Clicked -= _inventoryAddAction;
        }
    }
}