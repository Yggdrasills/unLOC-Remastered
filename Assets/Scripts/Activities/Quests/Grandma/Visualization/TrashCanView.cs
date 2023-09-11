using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Grandma.Visualization
{
    public class TrashCanView : MonoBehaviour
    {
        public TrashCans BindItem => _bindItem;

        [SerializeField]
        private TrashCans _bindItem;
    }
}