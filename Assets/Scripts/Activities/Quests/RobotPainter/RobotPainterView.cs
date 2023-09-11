using DG.Tweening;

using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    [RequireComponent(typeof(InteractableItem))]
    public class RobotPainterView : MonoBehaviour
    {
        [SerializeField]
        private float _yOffset = 0.06f;

        [SerializeField]
        private float _duration = 0.2f;

        private InteractableItem _interactableItem;

        private Tween _tween;

        private void OnValidate()
        {
            if (_interactableItem == null)
            {
                _interactableItem = GetComponent<InteractableItem>();
            }
        }

        private void OnDisable()
        {
            if (_tween.IsActive())
                _tween.Kill();
        }

        public void Play()
        {
            _tween = transform.DOLocalMoveY(transform.position.y + _yOffset, _duration)
                .SetLoops(-1);

            _interactableItem.Disable();
        }
    }
}