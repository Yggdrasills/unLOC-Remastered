using System;
using System.Collections.Generic;

using SevenDays.Localization;

namespace SevenDays.DialogSystem.Runtime
{
    public class DialogService
    {
        private readonly LocalizationService _localizationService;
        private DialogModel _currentDialog;
        private Dictionary<string, Queue<Action>> _tagActions;

        public event Action DialogEnded;

        public DialogService(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void StartDialog(DialogWindow dialogWindow)
        {
            StopCurrentDialog();

            _tagActions ??= new Dictionary<string, Queue<Action>>();
            _currentDialog = new DialogModel(dialogWindow, _localizationService, _tagActions);
            _currentDialog.Ended += DialogEnded;
        }

        public void StopCurrentDialog()
        {
            _currentDialog?.Stop();
            _currentDialog = null;
        }

        public void SubscribeTagAction<TTag>(TTag dialogTag, Action action) where TTag : DialogTag
        {
            _tagActions ??= new Dictionary<string, Queue<Action>>();

            if (_tagActions.TryGetValue(dialogTag.ToString(), out var actionQueue))
            {
                actionQueue.Enqueue(action);
            }
            else
            {
                var actionQ = new Queue<Action>();
                actionQ.Enqueue(action);

                _tagActions.Add(dialogTag.ToString(), actionQ);
            }
        }
    }
}