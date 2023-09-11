using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;
using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer.Unity;

namespace SevenDays.unLOC.Activities.Intro
{
    [UsedImplicitly]
    public class IntroController : IStartable
    {
        private readonly LocalizationService _localization;
        private readonly IDialogView _dialogView;
        private readonly TextAsset _dialogAsset;
        private readonly SceneLoader _sceneLoader;

        public IntroController(LocalizationService localization,
            IDialogView dialogView,
            TextAsset dialogAsset,
            SceneLoader sceneLoader)
        {
            _localization = localization;
            _dialogView = dialogView;
            _dialogAsset = dialogAsset;
            _sceneLoader = sceneLoader;
        }

        public void Start()
        {
            var wrapper = new DialogWrapper(_localization);

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .OnComplete(() => _sceneLoader.LoadWorkshopAsync().Forget());

            wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }
    }
}