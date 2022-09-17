using System;
using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;

using Newtonsoft.Json;

using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer.Unity;

using Object = UnityEngine.Object;

namespace SevenDays.unLOC.Inventory.Services
{
    public class ItemClickStrategy
    {
        public InventoryItem Type { get; set; } = InventoryItem.None;

        public Action ClickStrategy { get; set; }
    }

    public class InventoryService : IInventoryService, IInitializable, IDisposable
    {
        private readonly List<ItemClickStrategy> _clickStrategies;

        private readonly Dictionary<Item, InventoryCellView> _inventory;

        private readonly InventoryCellView _cellPrefab;

        private readonly InventoryView _inventoryView;

        private readonly DataStorage _storage;

        public InventoryService(
            InventoryCellView cellPrefab,
            InventoryView inventoryView,
            DataStorage storage)
        {
            _clickStrategies = new List<ItemClickStrategy>()
            {
                new ItemClickStrategy() {Type = InventoryItem.None},
                new ItemClickStrategy() {Type = InventoryItem.Screwdriver},
                new ItemClickStrategy() {Type = InventoryItem.ScrewEdge3},
                new ItemClickStrategy() {Type = InventoryItem.ScrewRadiation},
                new ItemClickStrategy() {Type = InventoryItem.ScrewSpanner},
                new ItemClickStrategy() {Type = InventoryItem.Bottle},
                new ItemClickStrategy() {Type = InventoryItem.Wires},
                new ItemClickStrategy() {Type = InventoryItem.Glasses},
                new ItemClickStrategy() {Type = InventoryItem.Condenser},
            };

            _inventory = new Dictionary<Item, InventoryCellView>();
            _storage = storage;

            _cellPrefab = cellPrefab;
            _inventoryView = inventoryView;
        }

        void IInitializable.Initialize()
        {
            if (!_storage.TryLoad(typeof(InventoryService).FullName, out Item[] items))
            {
                return;
            }

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Icon = SpriteConverter.GetSprite(items[i].SerializableTexture);

                Add(items[i]);
            }
        }

        void IDisposable.Dispose()
        {
            var keys = _inventory.Keys.ToArray();

            _storage.Save(typeof(InventoryService).FullName, keys);
        }

        bool IInventoryService.Contains(InventoryItem type)
        {
            return _inventory.Keys.Any(k => k.Type == type);
        }

        void IInventoryService.Use(InventoryItem type)
        {
            if (TryGetCellItem(type, out var cellItem))
            {
                var amount = --cellItem.item.Amount;

                if (amount <= 0)
                {
                    RemoveItem(cellItem);
                }
                else
                {
                    cellItem.view.SetCounterText(amount);
                }
            }
            else
            {
                throw new Exception(
                    $"Method: {nameof(IInventoryService.Use)}. Dictionary doesn't contains typeof {type.ToString()}");
            }
        }

        void IInventoryService.SetClickStrategy(InventoryItem type, Action strategy)
        {
            var itemStrategy = _clickStrategies.FirstOrDefault(it => it.Type == type);

            if (itemStrategy == null)
            {
                return;
            }

            itemStrategy.ClickStrategy = strategy;
        }

        async UniTask IInventoryService.AddAsync(PickableBase pickable)
        {
            await pickable.VisualisePickAsync();

            var item = _inventory.Keys.SingleOrDefault(i => i.Type == pickable.Type) ??
                       new Item()
                       {
                           SerializableTexture = SpriteConverter.GetTexture(pickable.Icon),
                           Icon = pickable.Icon,
                           Type = pickable.Type
                       };

            item.Amount++;

            Add(item);
        }

        void IInventoryService.Remove(InventoryItem type)
        {
            if (TryGetCellItem(type, out var cellItem))
            {
                RemoveItem(cellItem);
            }
        }

        private void RemoveItem((Item item, InventoryCellView view) cellItem)
        {
            Object.Destroy(cellItem.view.gameObject);

            _inventory.Remove(cellItem.item);
        }

        private void Add(Item item)
        {
            if (!_inventory.ContainsKey(item))
            {
                var cellView = CreateView(item);

                _inventory.Add(item, cellView);
            }

            _inventory[item].SetCounterText(item.Amount);
        }

        private InventoryCellView CreateView(Item item)
        {
            var cellItem = Object.Instantiate(_cellPrefab, _inventoryView.Container);

            cellItem.SetIcon(item.Icon);
            cellItem.SetClickAction(_clickStrategies
                .FirstOrDefault(it => it.Type == item.Type));

            return cellItem;
        }

        private bool TryGetCellItem(InventoryItem type, out (Item item, InventoryCellView view) cellView)
        {
            var key = _inventory.Keys.SingleOrDefault(k => k.Type == type);

            cellView = key is null ? (null, null) : (key, _inventory[key]);

            return key != null;
        }

        private class Item
        {
            [JsonIgnore]
            public Sprite Icon { get; set; }

            public SerializableTexture SerializableTexture { get; set; }

            public InventoryItem Type { get; set; }

            public int Amount { get; set; }
        }
    }
}