using System;
using System.Collections.Generic;

using Ink.Runtime;

using SevenDays.InkWrapper.Models;

using UnityEngine;

namespace SevenDays.InkWrapper.Core
{
    public class DialogBuilder
    {
        private readonly Dialog _dialog;

        public DialogBuilder()
        {
            _dialog = new Dialog();
        }

        public DialogBuilder(TextAsset asset) : this()
        {
            _dialog.Asset = asset;
        }

        public DialogBuilder SetTextAsset(TextAsset asset)
        {
            _dialog.Asset = asset;

            return this;
        }

        public DialogBuilder SetAction(string tag, Action action)
        {
            if (!_dialog.TagActions.ContainsKey(tag))
            {
                _dialog.TagActions[tag] = new Queue<Action>();
            }

            _dialog.TagActions[tag].Enqueue(action);

            return this;
        }

        public DialogBuilder OnComplete(Action action)
        {
            _dialog.Completed = action;

            return this;
        }

        public DialogBuilder WithGlobalParameter(string tag, object value)
        {
            // todo: add logic
            return this;
        }

        public DialogBuilder WithGlobalObserver(string tag, Story.VariableObserver globalVariableObserver)
        {
            // todo: add logic
            return this;
        }

        public static implicit operator Dialog(DialogBuilder builder)
        {
            return builder._dialog;
        }
    }
}