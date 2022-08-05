using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;

using VContainer;

namespace SevenDays.unLOC.Core
{
    public class WorkshopCompositeRoot : AutoInjectableLifeTimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterDialogues(builder);
        }

        private void RegisterDialogues(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }
    }
}