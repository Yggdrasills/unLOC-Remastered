using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class LocBoxView : InteractableItem
    {
        [SerializeField]
        private FlowerQuest _flowerQuest;

        [SerializeField]
        private GameObject _screwCanvas;

        protected override void Enabled()
        {
            Clicked += OnClick;
        }

        protected override void Disabled()
        {
            Clicked -= OnClick;
        }

        private void OnClick()
        {
            if (_flowerQuest.FirstStagePassed)
            {
                _screwCanvas.SetActive(true);
            }
            else
            {
                // todo: play dialogue
            }
        }
    }
}