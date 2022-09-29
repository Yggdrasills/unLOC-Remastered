using System;
using System.Collections.Generic;

using Ink.Runtime;

using UnityEngine;

namespace SevenDays.InkWrapper.Models
{
    public class Dialog
    {
        internal Action Completed { private get; set; }

        internal TextAsset Asset
        {
            set => Story = new Story(value.text);
        }

        internal Dictionary<string, Queue<Action>> TagActions { get; }

        internal Story Story { get; private set; }

        internal Dialog()
        {
            TagActions = new Dictionary<string, Queue<Action>>();
        }

        public void Complete()
        {
            Completed?.Invoke();
        }
    }
}