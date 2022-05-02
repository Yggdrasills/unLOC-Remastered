using SevenDays.DialogSystem.Runtime;

using VContainer.Unity;

namespace Activities.Intro
{
    public class IntroController : IStartable
    {
        private readonly IntroView _introView;
        private readonly DialogService _dialogService;

        public IntroController(
            IntroView introView,
            DialogService dialogService)
        {
            _introView = introView;
            _dialogService = dialogService;
        }

        public void Start()
        {
            _dialogService.StartDialog(_introView.DialogWindow);
        }
    }
}