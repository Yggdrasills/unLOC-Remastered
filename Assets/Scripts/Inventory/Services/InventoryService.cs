using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Views;

using UnityEngine;

namespace SevenDays.unLOC.Services
{
    public class InventoryService
    {
        private readonly Dictionary<InventoryItem, InventoryCellView> _inventoryItems;

        private readonly InventoryCellView _cellPrefab;

        private readonly InventoryView _inventoryView;

        private readonly List<PickableBaseView> _pickableBaseViews;

        public InventoryService(
            InventoryCellView cellPrefab,
            InventoryView inventoryView)
        {
            _inventoryItems = new Dictionary<InventoryItem, InventoryCellView>();
            _pickableBaseViews = new List<PickableBaseView>();
            _cellPrefab = cellPrefab;
            _inventoryView = inventoryView;
        }

        public bool Use(InventoryItem type)
        {
            if (_inventoryItems.ContainsKey(type))
            {
                _inventoryItems[type].DecrementAmount();

                if (_inventoryItems[type].Amount <= 0)
                {
                    Object.Destroy(_inventoryItems[type].gameObject);
                    _inventoryItems.Remove(type);
                }

                return true;
            }

            return false;
        }

        public void HandlePickable(PickableBaseView pickableBaseView)
        {
            pickableBaseView.Clicked += () => OnPickableClickAsync(pickableBaseView).Forget();
        }

        private async UniTaskVoid OnPickableClickAsync(PickableBaseView pickableBaseView)
        {
            if (_pickableBaseViews.Contains(pickableBaseView))
                return;

            await pickableBaseView.VisualisePickAsync();

            var key = pickableBaseView.Type;

            if (!_inventoryItems.ContainsKey(key))
            {
                var cellItem = CreateItemAsync(pickableBaseView);
                _inventoryItems.Add(key, cellItem);
            }

            _inventoryItems[key].IncrementAmount();

            _pickableBaseViews.Add(pickableBaseView);
        }

        private InventoryCellView CreateItemAsync(PickableBaseView pickableBaseView)
        {
            var cellItem = Object.Instantiate(_cellPrefab, _inventoryView.Container);

            cellItem.SetIcon(pickableBaseView.Icon);

            cellItem.SetClickAction(pickableBaseView.GetInventoryClickStrategy());

            return cellItem;
        }
    }
}