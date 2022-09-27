using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Views.Choices;

namespace SevenDays.InkWrapper.Views.Dialogs
{
    public interface IDialogChoiceView : IDialogView
    {
        void HideChoices();

        UniTask PrependShowAsync();

        IChoiceButtonView GetChoice();
    }
}