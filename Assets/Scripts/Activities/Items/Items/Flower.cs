using SevenDays.unLOC.Activities.Quests.Flower;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Items.Items
{
    public class Flower : InteractableItem
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