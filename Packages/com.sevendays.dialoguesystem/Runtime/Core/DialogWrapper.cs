using Cysharp.Threading.Tasks;

using Ink.Runtime;

using SevenDays.InkWrapper.Models;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;

using UnityEngine.Assertions;

namespace SevenDays.InkWrapper.Core
{
    public class DialogWrapper
    {
        private readonly LocalizationService _localization;

        public DialogWrapper(LocalizationService localization)
        {
            _localization = localization;
        }

        public static DialogBuilder CreateDialog()
        {
            return new DialogBuilder();
        }

        public async UniTask StartDialogue(Dialog dialog, IDialogView viewBase)
        {
            Assert.IsNotNull(dialog.Story, $"Dialog story is null");

            await viewBase.ShowAsync();

            // todo: надо отписываться
            viewBase.Clicked += () => RevealStory(dialog, viewBase);

            RevealStory(dialog, viewBase);
        }

        private void RevealStory(Dialog dialog, IDialogView viewBase)
        {
            var story = dialog.Story;

            if (!story.canContinue)
            {
                dialog.Complete();

                viewBase.HideAsync().Forget();

                return;
            }

            var storyText = story.Continue();

            CheckTags(dialog);

            if (string.IsNullOrEmpty(storyText))
            {
                return;
            }

            var textToReveal = _localization.GetLocalizedLine(storyText);

            viewBase.RevealAsync(textToReveal).Forget();

            if (viewBase is IDialogChoiceView choiceView)
            {
                ShowChoices(dialog, choiceView);
            }
        }

        private void CheckTags(Dialog dialog)
        {
            foreach (var tag in dialog.Story.currentTags)
            {
                if (dialog.TagActions.TryGetValue(tag, out var actionQueue))
                {
                    foreach (var action in actionQueue)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        private void ShowChoices(Dialog dialog, IDialogChoiceView viewBase)
        {
            var story = dialog.Story;

            if (story.currentChoices.Count <= 0)
                return;

            foreach (var choice in story.currentChoices)
            {
                var choiceText = _localization.GetLocalizedLine(choice.text).Trim();

                var choiceView = viewBase.CreateChoice();

                choiceView.SetText(choiceText);

                var choice1 = choice;
                choiceView.SetClickAction(() => OnClickChoiceButton(dialog, viewBase, choice1));

                choiceView.Show();
            }
        }

        private void OnClickChoiceButton(Dialog dialog, IDialogChoiceView viewBase, Choice choice)
        {
            if (viewBase.IsRevealing)
            {
                viewBase.Reveal();

                return;
            }

            dialog.Story.ChooseChoiceIndex(choice.index);

            viewBase.RemoveChoices();

            RevealStory(dialog, viewBase);
        }
    }
}