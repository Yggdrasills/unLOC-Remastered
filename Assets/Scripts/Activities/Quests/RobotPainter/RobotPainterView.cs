using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class RobotPainterView : MonoBehaviour
    {
        [SerializeField]
        private float _yOffset = 0.06f;

        [SerializeField]
        private float _duration = 0.2f;

        private Tween _tween;

        private void OnDisable()
        {
            if (_tween.IsActive())
                _tween.Kill();
        }

        public void Play()
        {
            _tween = transform.DOLocalMoveY(transform.position.y + _yOffset, _duration)
                .SetLoops(-1);
        }
    }
}