using System.Collections.Generic;

using DialogSystem.Localization;

using Ink.Runtime;

using UnityEngine;

namespace DialogSystem.Runtime
{
    public class DialogController
    {
        private readonly DialogWindow _window;
        private readonly LocalizationService _localizationService;
        private readonly DialogModel _model;
        private Story _story;

        public DialogController(
            DialogWindow window,
            LocalizationService localizationService)
        {
            _window = window;
            _localizationService = localizationService;
            _model = new DialogModel();

            Start();
        }

        public void Stop()
        {
            _window.Hide();
            _story = null;
        }

        private void Start()
        {
            _story = new Story(_window.DialogJson.text);
            
            CheckGlobalTags();
            
            _window.Show();
            UpdateWindow();
        }
        
         private void UpdateWindow()
        {
            _window.Reset();

            while (_story.canContinue)
            {
                var storyText = _story.Continue();
                
                CheckTags(_story.currentTags);
                
                var localizedText = _localizationService.GetLocalizedLine(storyText).Trim();
                
                _window.SetText(localizedText);
            }

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
             if(_story.globalTags is null)
                 return;

             if (_story.globalTags.Count is 0)
                 return;
             
             CheckTags(_story.globalTags);
         }
         
         private void CheckTags(List<string> tags)
         {
             // todo: implement event bus
             
             if(tags.Contains(DialogTags.EngLocalization))
                 _localizationService.SetCurrentLanguage(Language.English);
             
             if(tags.Contains(DialogTags.DefaultLocalization))
                 _localizationService.SetCurrentLanguage(Language.Default);
         }

         private void OnClickChoiceButton(Choice choice)
        {
            _story.ChooseChoiceIndex(choice.index);
            UpdateWindow();
        }
    }
}