using SevenDays.Screens.Models;
using SevenDays.Screens.Services;
using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Core.Scopes
{
    public class CoreLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private ScreenCollection _screenCollection;

        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInventory(builder);

            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.Register<IScreenService, ScreenService>(Lifetime.Singleton)
                .WithParameter(_screenCollection);
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