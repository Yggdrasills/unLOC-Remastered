using System;
using System.Collections.Generic;

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
        private readonly Dictionary<IDialogView, Action> _dialogSubscriptions;

        public DialogWrapper(LocalizationService localization)
        {
            _localization = localization;
            _dialogSubscriptions = new Dictionary<IDialogView, Action>();
        }

        public static DialogBuilder CreateDialog()
        {
            return new DialogBuilder();
        }

        public async UniTaskVoid StartDialogueAsync(Dialog dialog, IDialogView view)
        {
            Assert.IsNotNull(dialog.Story, $"Dialog story is null");

            await view.ShowAsync();

            _dialogSubscriptions[view] = () => RevealStoryAsync(dialog, view).Forget();

            view.Clicked += _dialogSubscriptions[view];

            RevealStoryAsync(dialog, view).Forget();
        }

        private async UniTaskVoid RevealStoryAsync(Dialog dialog, IDialogView view)
        {
            var story = dialog.Story;

            if (story.currentChoices.Count > 0)
            {
                view.Reveal();

                return;
            }

            if (!story.canContinue)
            {
                if (_dialogSubscriptions.ContainsKey(view))
                {
                    view.Clicked -= _dialogSubscriptions[view];
                }

                dialog.Complete();

                view.HideAsync().Forget();

                return;
            }

            var storyText = story.Continue();

            CheckTags(dialog);

            if (string.IsNullOrEmpty(storyText))
            {
                return;
            }

            var textToReveal = _localization.GetLocalizedLine(storyText);

            view.RevealAsync(textToReveal).Forget();

            if (view is IDialogChoiceView choiceView)
            {
                await ShowChoicesAsync(dialog, choiceView);
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

        private async UniTask ShowChoicesAsync(Dialog dialog, IDialogChoiceView view)
        {
            var story = dialog.Story;

            if (story.currentChoices.Count <= 0)
                return;

            await view.PrependShowAsync();

            foreach (var choice in story.currentChoices)
            {
                var choiceText = _localization.GetLocalizedLine(choice.text).Trim();

                var choiceView = view.GetChoice();

                choiceView.SetText(choiceText);

                choiceView.SetClickAction(() => OnChoiceClick(dialog, view, choice));

                choiceView.Show();
            }
        }

        private void OnChoiceClick(Dialog dialog, IDialogChoiceView view, Choice choice)
        {
            if (view.IsRevealing)
            {
                view.Reveal();

                return;
            }

            dialog.Story.ChooseChoiceIndex(choice.index);

            view.HideChoices();

            RevealStoryAsync(dialog, view).Forget();
        }
    }
}