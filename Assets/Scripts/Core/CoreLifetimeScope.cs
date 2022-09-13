using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Core
{
    public class CoreLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInventory(builder);
        }

        private void RegisterInventory(IContainerBuilder builder)
        {
            builder.Register<InventoryService>(Lifetime.Singleton)
                .WithParameter(_cellPrefab)
                .WithParameter(_inventoryView)
                .AsImplementedInterfaces();
        }
    }
}