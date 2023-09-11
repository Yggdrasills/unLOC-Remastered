using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using SevenDays.Screens.Views;
using SevenDays.unLOC.Utils.Extensions;

using UnityEngine;

namespace SevenDays.unLOC.Screens.Views
{
    internal sealed class FadeScreenView : DefaultScreenView
    {
        [SerializeField]
        private float _showDuration = 0.75f;

        [SerializeField]
        private float _hideDuration = 0.25f;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private CancellationToken _cancellationToken;

        private void Awake()
        {
            _cancellationToken = gameObject.GetCancellationTokenOnDestroy();
        }

        protected override void Validated()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        protected override async UniTask VisualizeShowAsync(CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource
                .CreateLinkedTokenSource(_cancellationToken, cancellationToken);

            await _canvasGroup.DOFade(1, _showDuration)
                .AwaitWithCancellation(cts.Token);
        }

        protected override async UniTask VisualizeHideAsync(CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource
                .CreateLinkedTokenSource(_cancellationToken, cancellationToken);

            await _canvasGroup.DOFade(0, _hideDuration)
                .AwaitWithCancellation(cts.Token);
        }
    }
}