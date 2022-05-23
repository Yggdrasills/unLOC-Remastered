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
        private string _selfPraiseText = "Гениально. Пора включать тебя, старикан";

        // todo: подумать на названием
        public UniTask DisplayTooEasySarcasm()
        {
            return Display(_noWiresText);
        }

        public UniTask DisplayForgotWires()
        {
            return Display(_forgotWiresText);
        }

        public UniTask DisplaySelfPraise()
        {
            return Display(_selfPraiseText);
        }

        private async UniTask Display(string text)
        {
            _dialogWindow.RevealImmediately();
            _dialogWindow.ResetToDefault();
            await _dialogWindow.ShowTextAsync(text);
        }
    }
}