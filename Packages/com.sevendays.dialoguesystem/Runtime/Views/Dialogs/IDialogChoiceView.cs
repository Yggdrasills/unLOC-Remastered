using SevenDays.InkWrapper.Views.Choices;

namespace SevenDays.InkWrapper.Views.Dialogs
{
    public interface IDialogChoiceView : IDialogView
    {
        void RemoveChoices();

        IChoiceButtonView CreateChoice();
    }
}