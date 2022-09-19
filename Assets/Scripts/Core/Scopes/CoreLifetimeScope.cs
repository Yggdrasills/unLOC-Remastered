using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;
using SevenDays.Screens.Models;
using SevenDays.Screens.Services;
using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Profiles.Services;
using SevenDays.unLOC.Storage;
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
        private GameServiceData _serviceData;

        [SerializeField]
        private Camera _mainCamera;

        [SerializeField]
        private Transform _screenCanvasTransform;

        protected override void Configure(IContainerBuilder builder)
        {
            IStorageDecorator storage = new StorageDecorator();

            builder.Register<IScreenService, ScreenService>(Lifetime.Singleton)
                .WithParameter(_screenCollection)
                .WithParameter(_screenCanvasTransform);

            builder.RegisterInstance<IStorageRepository>(storage);

            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();

            builder.Register<ProfileService>(Lifetime.Singleton)
                .WithParameter(storage)
                .AsImplementedInterfaces();

            builder.Register<SceneLoader>(Lifetime.Singleton)
                .WithParameter(_loadingScreen);

            builder.RegisterEntryPoint<CoreStartup>();

            builder.RegisterInstance(_serviceData);
            builder.RegisterComponent(_mainCamera);
        }
    }
}