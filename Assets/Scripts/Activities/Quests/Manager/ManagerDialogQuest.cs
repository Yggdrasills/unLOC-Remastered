using SevenDays.unLOC.Activities.Workshop.Views;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Activities.Quests.Manager
{
    public class ManagerDialogQuest : DialogQuest
    {
        [SerializeField]
        private Button _managerButton;

        [SerializeField]
        private GameObject _content;

        protected override void Initialized()
        {
            if (Storage.IsExists(GetType().FullName))
            {
                _managerButton.gameObject.SetActive(false);

                return;
            }

            _managerButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _content.SetActive(false);
            BeginDialog();
        }

        private void BeginDialog()
        {
            if (!Storage.IsExists(typeof(NoteView).FullName))
            {
                return;
            }

            _managerButton.gameObject.SetActive(false);

            StartDialog();
        }
    }
}