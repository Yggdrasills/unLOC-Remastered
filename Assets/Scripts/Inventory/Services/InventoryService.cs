﻿using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Inventory.Views;

using Object = UnityEngine.Object;

namespace SevenDays.unLOC.Inventory.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly Dictionary<InventoryItem, InventoryCellView> _inventoryItems;

        private readonly InventoryCellView _cellPrefab;

        private readonly InventoryView _inventoryView;

        private readonly List<PickableBase> _pickableBaseViews;

        public InventoryService(
            InventoryCellView cellPrefab,
            InventoryView inventoryView)
        {
            _inventoryItems = new Dictionary<InventoryItem, InventoryCellView>();
            _pickableBaseViews = new List<PickableBase>();
            _cellPrefab = cellPrefab;
            _inventoryView = inventoryView;
        }

        bool IInventoryService.Contains(InventoryItem type)
        {
            return _inventoryItems.ContainsKey(type);
        }

        void IInventoryService.Use(InventoryItem type)
        {
            if (_inventoryItems.ContainsKey(type))
            {
                _inventoryItems[type].DecrementAmount();

                if (_inventoryItems[type].Amount <= 0)
                {
                    RemoveItem(type);
                }
            }
            else
            {
                throw new Exception(
                    $"Method: {nameof(IInventoryService.Use)}. Dictionary doesn't contains typeof {type.ToString()}");
            }
        }

        async UniTask IInventoryService.AddAsync(PickableBase pickable)
        {
            await AddPickableAsync(pickable);
        }

        void IInventoryService.Remove(InventoryItem type)
        {
            if (_inventoryItems.ContainsKey(type))
            {
                RemoveItem(type);
            }
        }

        private void RemoveItem(InventoryItem type)
        {
            Object.Destroy(_inventoryItems[type].gameObject);
            _inventoryItems.Remove(type);
        }

        private async UniTask AddPickableAsync(PickableBase pickableBase)
        {
            await pickableBase.VisualisePickAsync();

            var key = pickableBase.Type;

            if (!_inventoryItems.ContainsKey(key))
            {
                var cellItem = CreateItemAsync(pickableBase);
                _inventoryItems.Add(key, cellItem);
            }

            _inventoryItems[key].IncrementAmount();

            if (!_pickableBaseViews.Contains(pickableBase))
                _pickableBaseViews.Add(pickableBase);
        }

        private InventoryCellView CreateItemAsync(PickableBase pickableBase)
        {
            var cellItem = Object.Instantiate(_cellPrefab, _inventoryView.Container);

            cellItem.SetIcon(pickableBase.Icon);

            cellItem.SetClickAction(pickableBase.GetInventoryClickStrategy());

            return cellItem;
        }
    }
}