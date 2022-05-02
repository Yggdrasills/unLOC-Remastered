using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Moving.Demo
{
    public class DemoMovingLifetimeScope : LifetimeScope
    {
        [SerializeField] private TapZoneView _tapZoneView;
        [SerializeField] private DemoPlayerView _demoPlayerView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_demoPlayerView).AsImplementedInterfaces();
            builder.Register<IMovingService, MovingService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DemoMovingController>();
        }
    }
}