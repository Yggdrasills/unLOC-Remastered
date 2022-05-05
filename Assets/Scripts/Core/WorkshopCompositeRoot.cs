using SevenDays.unLOC.Core.Moving;
using SevenDays.unLOC.Core.Moving.Demo;
using SevenDays.unLOC.Services;
using SevenDays.unLOC.Views;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core
{
    public class WorkshopCompositeRoot : LifetimeScope
    {
        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        [SerializeField]
        private PickableBase[] _pickables;

        [SerializeField]
        private DemoPlayerView _demoPlayerView;

        [SerializeField]
        private TapZoneView _tapZoneView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInventory(builder);
            RegisterPlayerMovement(builder);
        }

        private void RegisterInventory(IContainerBuilder builder)
        {
            var inventoryService = new InventoryService(_cellPrefab, _inventoryView);

            foreach (var pickable in _pickables)
            {
                inventoryService.HandlePickable(pickable);
            }

            builder.RegisterInstance(inventoryService).AsImplementedInterfaces();
        }

        private void RegisterPlayerMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_demoPlayerView).AsImplementedInterfaces();

            builder.Register<IMovingService, MovingService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<DemoMovingController>();
        }
    }
}