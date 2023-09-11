using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Grandma.Visualization
{
    public class SmoothDisplacerView : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 0.2f;

        private Tween _moveTween;

        public void SetPosition(Vector2 targetPosition)
        {
            _moveTween?.Kill();

            _moveTween = transform.DOMove(targetPosition, _speed);
        }
    }
}