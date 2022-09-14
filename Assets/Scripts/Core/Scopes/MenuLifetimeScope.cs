using SevenDays.unLOC.Menu;
using SevenDays.unLOC.Utils.Helpers;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class MenuLifetimeScope : AutoInjectableLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MenuView>().AsSelf();
            builder.RegisterComponentInHierarchy<LoadingPanelView>().AsSelf();
            builder.RegisterEntryPoint<MenuController>();
        }
    }
}