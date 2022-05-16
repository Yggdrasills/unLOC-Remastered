using System;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Grandma.Visualization
{
    public class BounceScaler : MonoBehaviour
    {
        [SerializeField]
        private Transform _selfTransform;

        [SerializeField]
        private Vector3[] _bounceScales;

        [SerializeField]
        private float _duration = 0.5f;

        private Sequence _scaleSequence;
        private Vector3 _initScale;

        private void Awake()
        {
            _initScale = _selfTransform.localScale;
        }

        public void ResetToInit()
        {
            SetScale(_initScale);
        }

        /// <summary>
        /// Callback from inspector
        /// </summary>
        public void BounceScale()
        {
            BounceScale(_bounceScales, _duration);
        }

        public void BounceScale(Action onComplete)
        {
            BounceScale(_bounceScales, _duration, onComplete);
        }

        public void BounceScale(float duration, Action onComplete = null)
        {
            BounceScale(_bounceScales, duration, onComplete);
        }

        public void BounceScale(Vector3[] scales, Action onComplete = null)
        {
            BounceScale(scales, _duration, onComplete);
        }

        public void BounceScale(Vector3[] scales, float duration, Action onComplete = null)
        {
            int bouncesCount = scales.Length;
            float stepDuration = duration / bouncesCount;

            StopScaling();

            _scaleSequence = DOTween.Sequence();

            for (int i = 0; i < bouncesCount; i++)
            {
                _scaleSequence.Append(_selfTransform.DOScale((scales[i]), stepDuration));
            }

            _scaleSequence.OnComplete(() => onComplete?.Invoke());
        }

        public void SetTargetScale()
        {
            SetScale(_bounceScales[_bounceScales.Length - 1]);
        }

        public void SetScale(Vector3 scale)
        {
            StopScaling();

            _selfTransform.localScale = scale;
        }

        public void StopScaling()
        {
            _scaleSequence?.Kill();
        }

        public bool IsTweenActive()
        {
            if (_scaleSequence == null) return false;

            return _scaleSequence.IsActive();
        }
    }
}