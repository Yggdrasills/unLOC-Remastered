using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    [AddComponentMenu(nameof(LocBoxView) + " [Flower Quest]")]
    public class LocBoxView : InteractableItem
    {
        // todo: inverse dependency
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