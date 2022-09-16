using SevenDays.unLOC.Activities.Intro;
using SevenDays.unLOC.SaveSystem;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class IntroLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private IntroConfig _introConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_introConfig);

            builder.RegisterEntryPoint<IntroController>();
            builder.RegisterComponentInHierarchy<IntroView>();
            
            builder.UseSaveSystem();
        }
    }
}