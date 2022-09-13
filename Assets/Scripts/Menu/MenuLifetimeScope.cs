using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Menu
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuView>().AsSelf();
            builder.RegisterComponentInHierarchy<LoadingPanelView>().AsSelf();
            builder.RegisterEntryPoint<MenuController>();
        }
    }
}