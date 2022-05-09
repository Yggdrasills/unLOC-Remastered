using System.Linq;

using SevenDays.unLOC.Activities.Quests;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Workshop
{
    public class ExitDoorView : MonoBehaviour
    {
        [SerializeField]
        private QuestBase[] _quests;

        public void GoToStreet()
        {
            if (_quests.Any(t => !t.IsCompleted))
            {
                // todo: dialogue about not all quests completed
                return;
            }
            // todo: load street scene
        }
    }
}