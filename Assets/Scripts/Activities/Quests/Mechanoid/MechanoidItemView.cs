using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidItemView : ClickableItem
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Sprite _targetSprite;

        private Sprite _initSprite;

        private void OnValidate()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Awake()
        {
            _initSprite = _renderer.sprite;
        }

        public void SetSprite()
        {
            SetSprite(_targetSprite);
        }

        public void ResetToDefault()
        {
            SetSprite(_initSprite);
        }

        private void SetSprite(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }
    }
}