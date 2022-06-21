using SevenDays.DialogSystem.Components;
using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;
/*using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Movement.Demo;*/
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

        /*[SerializeField]
        private DemoPlayerView _demoPlayerView;

        [SerializeField]
        private TapZoneView _tapZoneView;*/

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AutoInjectableLifeTimeScope>()
                .AsImplementedInterfaces()
                .AsSelf();

            RegisterInventory(builder);
            // RegisterPlayerMovement(builder);
            RegisterDialogues(builder);
        }

        private void RegisterInventory(IContainerBuilder builder)
        {
            builder.Register<InventoryService>(Lifetime.Singleton)
                .WithParameter(_cellPrefab)
                .WithParameter(_inventoryView)
                .AsImplementedInterfaces();
        }

        /*private void RegisterPlayerMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_demoPlayerView).AsImplementedInterfaces();

            builder.Register<IMovementService, MovementService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<DemoMovementController>();
        }*/

        private void RegisterDialogues(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }
    }
}