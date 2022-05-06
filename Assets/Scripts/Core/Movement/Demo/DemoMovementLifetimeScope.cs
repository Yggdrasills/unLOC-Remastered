using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class DemoMovementLifetimeScope : LifetimeScope
    {
        [SerializeField] private TapZoneView _tapZoneView;
        [SerializeField] private DemoPlayerView _demoPlayerView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_demoPlayerView).AsImplementedInterfaces();
            builder.Register<IMovementService, MovementService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DemoMovementController>();
        }
    }
}