using SevenDays.unLOC.Core.Animations;
using SevenDays.unLOC.Core.Animations.Config;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class DemoMovementLifetimeScope : LifetimeScope
    {
        [SerializeField] private TapZoneView _tapZoneView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private AnimationConfig _animationConfig;
        

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tapZoneView);
            builder.RegisterInstance(_animationConfig);
            builder.RegisterInstance(_playerView).AsImplementedInterfaces();
            builder.Register<IMovementService, MovementService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<InputService>().AsSelf();
            builder.RegisterEntryPoint<MovementController>();
            builder.RegisterEntryPoint<PlayerAnimationController>();
        }
    }
}