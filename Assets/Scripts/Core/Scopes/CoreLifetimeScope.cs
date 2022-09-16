using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;
using SevenDays.Screens.Models;
using SevenDays.Screens.Services;
using SevenDays.unLOC.Activities.Items.Pad;
using SevenDays.unLOC.Activities.Quests.Flower.Screwdriver;
using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;
using SevenDays.unLOC.Profiles.Services;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class CoreLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private ScreenIdentifier _loadingScreen;

        [SerializeField]
        private ScreenCollection _screenCollection;

        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        [SerializeField]
        private PadView _padView;

        [SerializeField]
        private ScrewdriverView _screwdriverView;

        [SerializeField]
        private Transform _screenCanvasTransform;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_padView);
            builder.RegisterComponent(_screwdriverView);

            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();

            builder.RegisterEntryPoint<ProfileService>();

            builder.Register<InventoryService>(Lifetime.Singleton)
                .WithParameter(_cellPrefab)
                .WithParameter(_inventoryView)
                .AsImplementedInterfaces();

            builder.Register<SceneLoader>(Lifetime.Singleton)
                .WithParameter(_loadingScreen);

            builder.Register<IScreenService, ScreenService>(Lifetime.Singleton)
                .WithParameter(_screenCollection)
                .WithParameter(_screenCanvasTransform);

            builder.RegisterEntryPoint<CoreStartup>();
        }

        private void RegisterDialogues(IContainerBuilder builder)
        {
        }
    }
}