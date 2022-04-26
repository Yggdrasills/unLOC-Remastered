using SevenDays.Localization;

using UnityEngine;

using VContainer;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogSystemDemo : MonoBehaviour
    {
        [SerializeField] private DialogWindow _dialogWindow;

        private DialogService _dialogService;
        private LocalizationService _localizationService;

        [Inject]
        private void Construct(
            DialogService dialogService,
            LocalizationService localizationService)
        {
            _dialogService = dialogService;
            _localizationService = localizationService;
        }

        public void StartDialog()
        {
            _dialogService.SubscribeTagAction(DialogTag.EngLocalization, HandleEngLocalizationTagEvent);
            _dialogService.SubscribeTagAction(DialogTag.DefaultLocalization, HandleDefaultLocalizationTagEvent);

            _dialogService.StartDialog(_dialogWindow);
        }

        private void HandleEngLocalizationTagEvent()
        {
            _localizationService.SetCurrentLanguage(Language.English);
        }

        private void HandleDefaultLocalizationTagEvent()
        {
            _localizationService.SetCurrentLanguage(Language.Default);
        }
    }
}