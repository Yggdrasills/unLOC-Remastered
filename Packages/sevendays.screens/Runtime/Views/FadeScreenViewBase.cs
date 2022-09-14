using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.Screens.Views
{
    internal sealed class FadeScreenViewBase : ScreenViewBase
    {
        [SerializeField]
        private float _showDuration = 0.75f;

        [SerializeField]
        private float _hideDuration = 0.25f;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private GameObject _content;

        private void OnValidate()
        {
            if (_canvasGroup is null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            if (_content is null && transform.childCount > 0)
            {
                _content = transform.GetChild(0).gameObject;
            }
        }

        protected override async UniTask VisualizeShowAsync(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, _showDuration)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken);
        }

        protected override async UniTask VisualizeHideAsync(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, _hideDuration)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken);
        }

        protected override void Enable()
        {
            _content.SetActive(true);
        }

        protected override void Disable()
        {
            _content.SetActive(false);
        }
    }
}