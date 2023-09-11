using Activities.Credits;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class CreditsLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private ClickableItem _clickableItem;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CreditsStartup>()
                .WithParameter(_clickableItem);
        }
    }
}