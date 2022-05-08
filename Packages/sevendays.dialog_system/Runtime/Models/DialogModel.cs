using System;
using System.Collections.Generic;

using SevenDays.Localization;

using Ink.Runtime;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogModel
    {
        private readonly DialogWindow _window;
        private readonly LocalizationService _localizationService;

        private Story _story;
        private Dictionary<string, Queue<Action>> _tagActions;
        public event Action Ended = delegate { };

        public DialogModel(
            DialogWindow window,
            LocalizationService localizationService,
            Dictionary<string, Queue<Action>> tagActions)
        {
            _window = window;
            _localizationService = localizationService;
            _tagActions = tagActions;

            Start();
        }

        public void Stop()
        {
            _tagActions.Clear();
            _window.Hide();
            _story = null;
            _window.Clicked -= UpdateWindow;
        }

        private void Start()
        {
            _story = new Story(_window.DialogJson.text);
            _window.Clicked += UpdateWindow;

            CheckGlobalTags();

            _window.Show();
            UpdateWindow();
        }

        private void UpdateWindow()
        {
            if (_story.canContinue == false)
            {
               _window.Hide();
               Ended?.Invoke();
               return;
            }
            
            _window.Reset();

            var storyText = _story.Continue();

            CheckTags(_story.currentTags);

            var localizedText = _localizationService.GetLocalizedLine(storyText).Trim();

            _window.ShowText(localizedText);

            if (_story.currentChoices.Count > 0)
            {
                for (int i = 0; i < _story.currentChoices.Count; i++)
                {
                    var choice = _story.currentChoices[i];

                    var choiceText = _localizationService.GetLocalizedLine(choice.text).Trim();
                    var choiceView = _window.ChoiceViews[i];
                    choiceView.SetText(choiceText);
                    choiceView.Show();

                    choiceView.Button.onClick.AddListener(() => OnClickChoiceButton(choice));
                }
            }
        }

        private void CheckGlobalTags()
        {
            if (_story.globalTags is null)
                return;

            if (_story.globalTags.Count is 0)
                return;

            CheckTags(_story.globalTags);
        }

        private void CheckTags(List<string> tags)
        {
            foreach (var tag in tags)
            {
                if (_tagActions.TryGetValue(tag, out var actionQueue))
                {
                    foreach (var action in actionQueue)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        private void OnClickChoiceButton(Choice choice)
        {
            _story.ChooseChoiceIndex(choice.index);
            UpdateWindow();
        }
    }
}