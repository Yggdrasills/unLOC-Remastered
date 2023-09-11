using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Views.Dialogs;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidTextDisplayer : MonoBehaviour
    {
        [SerializeField]
        private DialogView _dialogWindow;

        [SerializeField]
        private string _noWiresText = "Да, если бы ты включился, было бы слишком просто";

        [SerializeField]
        private string _forgotWiresText = "Блин, забыл провода взять";

        [SerializeField]
        private string _selfPraiseText = "Гениально. Пора включать тебя, старикан...";

        [SerializeField]
        private string _timeToTurnOnText = "Попробую включить сейчас";

        [SerializeField]
        private string _wiresOnPlaceText = "Да тут уже все на месте";

        [SerializeField]
        private string _noPowerText = "Кажись чего-то забыл...";

        [SerializeField]
        private string _noCondenserText = "Где же у меня завалялся старенький конденсатор?..";

        private IDialogView _dialogView;

        private void Awake()
        {
            _dialogView = _dialogWindow;
        }

        // todo: подумать на названием
        public void DisplayTooEasySarcasm()
        {
            DisplayAsync(_noWiresText).Forget();
        }

        public void DisplayForgotWires()
        {
            DisplayAsync(_forgotWiresText).Forget();
        }

        public void DisplayTimeToTurnOn()
        {
            DisplayAsync(_timeToTurnOnText).Forget();
        }

        public void DisplaySelfPraise()
        {
            DisplayAsync(_selfPraiseText).Forget();
        }

        public void DisplayCantTurnOn()
        {
            DisplayAsync(_noPowerText).Forget();
        }

        public void DisplayWiresOnPlace()
        {
            DisplayAsync(_wiresOnPlaceText).Forget();
        }

        public void DisplayNoCondenser()
        {
            DisplayAsync(_noCondenserText).Forget();
        }

        private async UniTaskVoid DisplayAsync(string text)
        {
            await ResetToDefaultAsync();
            await _dialogView.ShowAsync();
            _dialogView.RevealAsync(text).Forget();
        }

        private async UniTask ResetToDefaultAsync()
        {
            _dialogView.Reveal();
            await _dialogView.HideAsync();
        }
    }
}