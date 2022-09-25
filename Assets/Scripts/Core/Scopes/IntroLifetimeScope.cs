using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.unLOC.Activities.Intro;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class IntroLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private TextAsset _dialogAsset;

        [SerializeField]
        private DialogView _dialogViewBase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<IntroController>()
                .WithParameter(_dialogAsset)
                .WithParameter(_dialogViewBase as IDialogView);
        }
    }
}