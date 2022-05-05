using System;
using System.Linq;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using TMPro;

using UnityEngine;

namespace SevenDays.unLOC.Screwdriver
{
    public class ScrewdriverView : MonoBehaviour
    {
        public Nozzle ActiveNozzle { get; private set; } = Nozzle.None;

        [SerializeField]
        private NozzlePair[] _nozzlePairs;

        [SerializeField]
        private float _targetPositionY = 220;

        [SerializeField]
        private float _initPositionY = 0;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private float _duration = 0.5f;

        private Tween _moveTween;

        private void OnDisable()
        {
            if (_moveTween != null && _moveTween.IsActive())
                _moveTween.Kill();
        }

        public async UniTaskVoid ShowAsync(Nozzle nozzle)
        {
            if (ActiveNozzle == nozzle || _moveTween != null && _moveTween.IsActive())
            {
                return;
            }

            if (ActiveNozzle != Nozzle.None)
            {
                await AnimateAsync(ActiveNozzle, _initPositionY);
            }

            AnimateAsync(nozzle, _targetPositionY).Forget();
        }

        private async UniTask AnimateAsync(Nozzle nozzle, float positionY)
        {
            var targetPair = _nozzlePairs.First(t => t.Nozzle == nozzle);

            if (targetPair.Transform == null)
                return;

            _moveTween = targetPair.Transform.DOAnchorPosY(positionY, _duration);

            ActiveNozzle = nozzle;

            _text.text = ActiveNozzle.ToString();

            await _moveTween.AsyncWaitForCompletion();
        }

        [Serializable]
        private class NozzlePair
        {
            public Nozzle Nozzle => _nozzle;

            public RectTransform Transform => _transform;

            [SerializeField]
            private RectTransform _transform;

            [SerializeField]
            private Nozzle _nozzle = Nozzle.None;
        }
    }
}