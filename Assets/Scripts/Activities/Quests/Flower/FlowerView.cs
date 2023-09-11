using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerView : InteractableItem
    {
        [SerializeField]
        private Sprite _repairedFlowerSprite;

        [SerializeField]
        private SpriteRenderer _renderer;

        public void SetFlowerRepairedSprite()
        {
            _renderer.sprite = _repairedFlowerSprite;
        }
    }
}