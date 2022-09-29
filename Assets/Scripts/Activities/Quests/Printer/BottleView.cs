using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Printer
{
    public class BottleView : PickableBase
    {
        [SerializeField]
        private InteractableItem _clickableItem;

        public override InventoryItem Type => InventoryItem.Bottle;

        private IInventoryService _inventory;
        private IStorageRepository _storage;

        private string _key;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory,
            IStorageRepository storage)
        {
            _inventory = inventory;
            _storage = storage;
        }

        private void OnValidate()
        {
            _clickableItem ??= GetComponent<InteractableItem>();
        }

        private void Start()
        {
            _key = typeof(BottleView).FullName + gameObject.name;

            if (_storage.IsExists(_key))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _clickableItem.Clicked += OnClick;
        }

        private void OnDisable()
        {
            _clickableItem.Clicked -= OnClick;
        }

        private void OnClick()
        {
            _storage.Save(_key, true);
            _inventory.AddAsync(this).Forget();
        }
    }
}