using Cysharp.Threading.Tasks;

using SevenDays.DialogSystem.Components;
using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;
using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Core.Moving;
using SevenDays.unLOC.Core.Moving.Demo;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

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
            builder.RegisterComponentInHierarchy<MonoBehaviourObjectResolver>()
                .AsImplementedInterfaces().AsSelf();
            builder.Register<InjectableMonoBehaviour>(Lifetime.Transient).AsSelf();

            RegisterInventory(builder);
            RegisterPlayerMovement(builder);
            RegisterDialogues(builder);
        }

        private void RegisterInventory(IContainerBuilder builder)
        {
            var inventoryService = new InventoryService(_cellPrefab, _inventoryView);

            /*foreach (var pickable in _pickables)
            {
                var clickable = pickable.GetComponent<ClickableItem>();

                clickable.Clicked += () => inventoryService.AddAsync(pickable).Forget();
            }*/

            builder.RegisterInstance(inventoryService).AsImplementedInterfaces().AsSelf();
        }

        private void RegisterPlayerMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_demoPlayerView).AsImplementedInterfaces();

            builder.Register<IMovingService, MovingService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<DemoMovingController>();
        }

        private void RegisterDialogues(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }
    }
}