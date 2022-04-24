using VContainer;
using VContainer.Unity;

namespace DialogSystem.Localization
{
    public class LocTestLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
        }
    }
}