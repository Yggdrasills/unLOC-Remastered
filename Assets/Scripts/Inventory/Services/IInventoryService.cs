using System;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Inventory.Views;

namespace SevenDays.unLOC.Inventory.Services
{
    public interface IInventoryService
    {
        bool Contains(InventoryItem type);

        void Use(InventoryItem type);

        void SetClickStrategy(InventoryItem type, Action strategy);

        UniTask AddAsync(PickableBase pickable);

        void Remove(InventoryItem item);
    }
}