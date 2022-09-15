using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

namespace SevenDays.Screens.Views
{
    internal sealed class FadeScreenView : DefaultScreenView
    {
        [SerializeField]
        private float _showDuration = 0.75f;

        [SerializeField]
        private float _hideDuration = 0.25f;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        protected override void Validated()
        {
            if (_canvasGroup is null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
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
    }
}