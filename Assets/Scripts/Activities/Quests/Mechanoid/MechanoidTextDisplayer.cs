using Cysharp.Threading.Tasks;

using SevenDays.DialogSystem.Runtime;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidTextDisplayer : MonoBehaviour
    {
        [SerializeField]
        private DialogWindow _dialogWindow;

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

        // todo: подумать на названием
        public UniTask DisplayTooEasySarcasm()
        {
            return Display(_noWiresText);
        }

        public UniTask DisplayForgotWires()
        {
            return Display(_forgotWiresText);
        }

        public UniTask DisplayTimeToTurnOn()
        {
            return Display(_timeToTurnOnText);
        }

        public UniTask DisplaySelfPraise()
        {
            return Display(_selfPraiseText);
        }

        public UniTask DisplayCantTurnOn()
        {
            return Display(_noPowerText);
        }

        public UniTask DisplayWiresOnPlace()
        {
            return Display(_wiresOnPlaceText);
        }

        public UniTask DisplayNoCondenser()
        {
            return Display(_noCondenserText);
        }

        public void ResetToDefault()
        {
            _dialogWindow.RevealImmediately();
            _dialogWindow.ResetToDefault();
        }

        private async UniTask Display(string text)
        {
            ResetToDefault();
            await _dialogWindow.ShowTextAsync(text);
        }
    }
}