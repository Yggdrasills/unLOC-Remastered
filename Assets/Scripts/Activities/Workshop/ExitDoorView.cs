using System.Linq;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SevenDays.unLOC.Activities.Workshop
{
    [RequireComponent(typeof(ClickableItem))]
    public class ExitDoorView : MonoBehaviour
    {
        [SerializeField]
        private ClickableItem _clickableItem;

        [SerializeField]
        private QuestBase[] _quests;

        private void OnValidate()
        {
            if (_clickableItem == null)
                _clickableItem = GetComponent<ClickableItem>();
        }

        private void OnEnable()
        {
            _clickableItem.Clicked += GoToStreet;
        }

        private void OnDisable()
        {
            _clickableItem.Clicked -= GoToStreet;
        }

        private void GoToStreet()
        {
            if (_quests.Any(t => !t.IsCompleted))
            {
                // todo: dialogue about not all quests completed
                return;
            }

            SceneManager.LoadScene(1);
        }
    }
}