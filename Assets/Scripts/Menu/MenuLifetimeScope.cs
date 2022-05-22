using VContainer;
using VContainer.Unity;

namespace Menu
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuView>().AsSelf();
            builder.RegisterEntryPoint<MenuController>();
        }
    }
}