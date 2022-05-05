using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerView : InteractableItem
    {
        [SerializeField]
        private FlowerQuest _flowerQuest;

        protected override void Enabled()
        {
            Clicked += ActivateQuest;
        }

        protected override void Disabled()
        {
            Clicked -= ActivateQuest;
        }

        private void ActivateQuest()
        {
            _flowerQuest.gameObject.SetActive(true);
        }
    }
}