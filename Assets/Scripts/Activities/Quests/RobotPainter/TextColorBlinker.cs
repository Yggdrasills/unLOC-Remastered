using Cysharp.Threading.Tasks;

using DG.Tweening;

using TMPro;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class TextColorBlinker : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _text;

        [SerializeField]
        private Color[] _colors;

        [SerializeField]
        private float _duration;

        private Sequence _blinkTween;

        private void OnDisable()
        {
            if (_blinkTween.IsActive())
                _blinkTween.Kill();
        }

        public async UniTask Blink(Color[] colors)
        {
            int blinksCount = colors.Length;
            float stepDuration = _duration / blinksCount;

            _blinkTween = DOTween.Sequence();

            for (int i = 0; i < blinksCount; i++)
            {
                _blinkTween.Append(_text.DOColor(colors[i], stepDuration));
            }

            await _blinkTween;
        }
    }
}