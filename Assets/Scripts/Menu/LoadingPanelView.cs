using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class LoadingPanelView : MonoBehaviour
    {
        [SerializeField] private Image _panel;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private Sequence _sequence;

        public async UniTask ShowAsync(float duration, CancellationToken token)
        {
            token.Register(() => GetSequence());
            _sequence = GetSequence();
            _sequence
                .Append(_panel.DOFade(1, 0.3f))
                .Join(_textMeshProUGUI.DOFade(1, 0.3f));
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.3f), cancellationToken: token);
        }

        public async UniTask HideAsync(float duration, CancellationToken token)
        {
            token.Register(() => GetSequence());
            _sequence = GetSequence();
            _sequence
                .Append(_panel.DOFade(0, 0.3f))
                .Join(_textMeshProUGUI.DOFade(0, 0.3f));
            await UniTask.Delay(TimeSpan.FromSeconds(duration + 0.3f), cancellationToken: token);
        }

        private Sequence GetSequence()
        {
            if (_sequence != null && _sequence.IsPlaying())
            {
                _sequence.Kill();
            }

            return DOTween.Sequence();
        }
    }
}