using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Inventory.Views;

namespace SevenDays.unLOC.Inventory.Services
{
    public interface IInventoryService
    {
        bool Contains(InventoryItem item);

        void Use(InventoryItem item);

        UniTask AddAsync(PickableBase pickable);

        void Remove(InventoryItem item);
    }
}