namespace SevenDays.unLOC.Services
{
    public interface IInventoryService
    {
        bool Contains(InventoryItem item);

        void Use(InventoryItem item);
    }
}