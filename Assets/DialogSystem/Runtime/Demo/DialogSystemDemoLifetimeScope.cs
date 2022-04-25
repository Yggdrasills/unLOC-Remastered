using DialogSystem.Localization;

using VContainer;
using VContainer.Unity;

namespace DialogSystem.Runtime
{
    public class DialogSystemDemoLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
            builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
        }
    }
}