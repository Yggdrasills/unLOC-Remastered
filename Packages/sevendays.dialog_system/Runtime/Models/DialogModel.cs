using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using Ink.Runtime;

using SevenDays.Localization;

using UnityEngine;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogModel
    {
        private readonly DialogWindow _window;
        private readonly LocalizationService _localizationService;
        private readonly Dictionary<string, Queue<Action>> _tagActions;

        private Story _story;
        private bool _isRevealing;
        private float _delayBetweenClicks = 0.1f;
        private float _lastClickTime;

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
            if(ClicksIsTooOften())
                return;

            if (_isRevealing)
            {
                _window.RevealImmediately();
                _isRevealing = false;

                ShowChoices();
                return;
            }

            if (_story.canContinue == false)
                return;

            if (_story.canContinue == false &&
                _story.currentChoices.Count == 0)
            {
                _window.Hide();
                _window.GetDialogueEndAction().Invoke();
                return;
            }

            _window.ResetToDefault();

            var storyText = _story.Continue();

            CheckTags(_story.currentTags);

            var localizedText = _localizationService.GetLocalizedLine(storyText).Trim();

            async UniTaskVoid UpdateWindowAsync()
            {
                _isRevealing = true;

                await _window.ShowTextAsync(localizedText);

                _isRevealing = false;
                ShowChoices();
            }

            UpdateWindowAsync().Forget();
        }

        private void ShowChoices()
        {
            if (_story.currentChoices.Count <= 0)
                return;

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

        private bool ClicksIsTooOften()
        {
            if (Time.time == 0) 
                return false;
            
            var value = Time.time - _lastClickTime < _delayBetweenClicks;
            _lastClickTime = Time.time;
            return value;
        }
    }
}