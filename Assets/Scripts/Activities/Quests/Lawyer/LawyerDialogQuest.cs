using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Activities.Quests.Lawyer
{
    public class LawyerDialogQuest : DialogQuest
    {
        [SerializeField]
        private Button _lawyerButton;

        [SerializeField]
        private GameObject _content;

        protected override void Initialized()
        {
            if (Storage.IsExists(GetType().FullName))
            {
                _lawyerButton.gameObject.SetActive(false);

                return;
            }

            _lawyerButton.onClick.AddListener(OnClick);
        }

        protected override Dictionary<string, Action> GetDialogTagActions()
        {
            return new Dictionary<string, Action>()
            {
                {"end1", () => { Debug.Log("Lawyer dialog first end"); }},
                {"end2", () => { Debug.Log("Lawyer dialog second end"); }},
            };
        }

        private void OnClick()
        {
            _content.SetActive(false);
            StartDialog();
        }
    }
}