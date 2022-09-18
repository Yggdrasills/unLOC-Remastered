using Cysharp.Threading.Tasks;

using DG.Tweening;

using TMPro;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class TextColorBlinker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private float _duration = 1f;

        private Sequence _blinkTween;

        private void OnValidate()
        {
            if (_text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void OnDisable()
        {
            if (_blinkTween.IsActive())
                _blinkTween.Kill();
        }

        public UniTask Blink(Color[] colors)
        {
            int blinksCount = colors.Length;
            float stepDuration = _duration / blinksCount;

            _blinkTween = DOTween.Sequence();

#pragma warning disable CS4014
            for (int i = 0; i < blinksCount; i++)
            {
                _blinkTween.Append(_text.DOColor(colors[i], stepDuration));
            }
#pragma warning restore

            return UniTask.CompletedTask;
        }
    }
}