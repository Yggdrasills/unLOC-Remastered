using SevenDays.Screens.Models;
using SevenDays.unLOC.Menu;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class MenuLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private ScreenIdentifier _menuScreen;

        [SerializeField]
        private ProfileLoadButton _profileLoadButtonPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MenuController>()
                .WithParameter(_profileLoadButtonPrefab)
                .WithParameter(_menuScreen);
        }
    }
}