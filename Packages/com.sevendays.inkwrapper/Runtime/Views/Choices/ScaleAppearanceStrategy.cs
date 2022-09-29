using DG.Tweening;

using UnityEngine;

namespace SevenDays.InkWrapper.Views.Choices
{
    public class ScaleAppearanceStrategy : AppearanceStrategy
    {
        [SerializeField]
        private Transform _content;

        [SerializeField]
        private float _appearDuration = 0.3f;

        private Tween _scaleTween;

        private void OnValidate()
        {
            if (_content == null)
            {
                if (transform.childCount > 0)
                {
                    _content = transform.GetChild(0);
                }
            }
        }

        private void Awake()
        {
            _content.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_scaleTween.IsActive())
            {
                _scaleTween.Kill();
            }
        }

        public override void Show()
        {
            _scaleTween = _content.DOScale(1, _appearDuration);
        }
    }
}