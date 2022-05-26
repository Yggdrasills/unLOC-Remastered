using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    [AddComponentMenu(nameof(LocBoxView) + " [Mechanoid Quest]")]
    public class LocBoxView : InteractableItem
    {
        [SerializeField]
        private MechanoidQuest _mechanoidQuest;

        [SerializeField]
        private GameObject _condenserCanvas;

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
            if (_mechanoidQuest.IsLastStage())
            {
                _condenserCanvas.SetActive(true);
            }
        }
    }
}