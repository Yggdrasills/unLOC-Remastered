using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Storage;

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

        private IStorageRepository _storage;
        private IInventoryService _inventory;

        private int _currentClickAmount;

        private string _storageKey;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory, IStorageRepository storage)
        {
            _inventory = inventory;
            _storage = storage;
        }

        private void Awake()
        {
            _storageKey = typeof(ScrewPickerView).FullName + _inventoryItem;

            if (_storage.TryLoad(_storageKey, out int clickAmount))
            {
                _currentClickAmount = clickAmount;

                if (_currentClickAmount >= _maxPickAmount)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            _storage.Save(_storageKey, _currentClickAmount);
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