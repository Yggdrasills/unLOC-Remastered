using DialogSystem.Localization;

namespace DialogSystem.Runtime
{
    public class DialogService
    {
        private readonly LocalizationService _localizationService;
        private DialogController _currentDialog;

        public DialogService(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        
        public void StartDialog(DialogWindow dialogWindow)
        {
            StopCurrentDialog();
            _currentDialog = new DialogController(dialogWindow, _localizationService);
        }

        public void StopCurrentDialog()
        {
            _currentDialog?.Stop();
            _currentDialog = null;
        }
    }
}