using Activities.Intro;

using SevenDays.DialogSystem.Runtime;
using SevenDays.Localization;

using UnityEngine;

using VContainer;
using VContainer.Unity;

public class IntroLifetimeScope : LifetimeScope
{
    [SerializeField] private IntroConfig _introConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_introConfig);
        
        builder.RegisterEntryPoint<IntroController>();
        builder.RegisterComponentInHierarchy<IntroView>();
        
        builder.Register<LocalizationService>(Lifetime.Singleton).AsSelf();
        builder.Register<DialogService>(Lifetime.Singleton).AsSelf();
    }
}
