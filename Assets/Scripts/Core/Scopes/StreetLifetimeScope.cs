using SevenDays.unLOC.Core.Player;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Core.Scopes
{
    public class StreetLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private InitializeConfig _initializeConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterCharacter(_camera, _initializeConfig, transform);
        }
    }
}