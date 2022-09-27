using System;

namespace SevenDays.InkWrapper.Views.Choices
{
    public interface IChoiceButtonView
    {
        public void SetClickAction(Action action);

        public void SetText(string text);

        public void Show();
    }
}