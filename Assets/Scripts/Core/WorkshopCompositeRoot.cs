using System.Collections.Generic;
using System.Linq;

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
        private Camera _camera;

        [SerializeField]
        private InitializeConfig _initializeConfig;

        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterDialogues(builder);
            RegisterMovement(builder);
        }

        private void RegisterDialogues(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_initializeConfig);
            IEnumerable<IInputModel> horizontalInput = new[]{new HorizontalInputModel()};
            builder.RegisterEntryPoint<InputController>().AsSelf().WithParameter(horizontalInput);
            builder.RegisterEntryPoint<PlayerCreator>()
                .WithParameter(_camera)
                .WithParameter(horizontalInput.First())
                .AsSelf();
        }
    }
}