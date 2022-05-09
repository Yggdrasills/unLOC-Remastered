using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests
{
    public class QuestBase : MonoBehaviour
    {
        public bool IsCompleted { get; private set; }

        protected void CompleteQuest()
        {
            IsCompleted = true;
        }
    }
}