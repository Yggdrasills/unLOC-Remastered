using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;
using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core
{
    public class WorkshopCompositeRoot : AutoInjectableLifeTimeScope
    {
        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private InitializeConfig _initializeConfig;


        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AutoInjectableLifeTimeScope>()
                .AsImplementedInterfaces()
                .AsSelf();

            RegisterInventory(builder);
            RegisterDialogues(builder);
            RegisterMovement(builder);
        }

        private void RegisterInventory(IContainerBuilder builder)
        {
            builder.Register<InventoryService>(Lifetime.Singleton)
                .WithParameter(_cellPrefab)
                .WithParameter(_inventoryView)
                .AsImplementedInterfaces();
        }

        private void RegisterDialogues(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_initializeConfig);
            builder.Register<IMovementService, MovementService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PlayerCreator>().WithParameter(_camera).AsSelf();
        }
    }
}