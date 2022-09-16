using System;
using System.Linq;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

using TMPro;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower.Screwdriver
{
    public class ScrewdriverView : MonoBehaviour
    {
        public Nozzle ActiveNozzle { get; private set; } = Nozzle.None;

        [SerializeField]
        private GameObject _content;

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

        private IInventoryService _inventory;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        private void Start()
        {
            _inventory.SetClickStrategy(InventoryItem.Screwdriver, ActivateContent);
        }

        private void OnDisable()
        {
            if (_moveTween.IsActive())
                _moveTween.Kill();
        }

        public void ShowAsync(Nozzle nozzle)
        {
            ShowAsync(nozzle, _duration).Forget();
        }

        private async UniTaskVoid ShowAsync(Nozzle nozzle, float duration)
        {
            if (ActiveNozzle == nozzle || _moveTween != null && _moveTween.IsActive())
            {
                return;
            }

            if (ActiveNozzle != Nozzle.None)
            {
                await AnimateAsync(ActiveNozzle, _initPositionY, duration);
            }

            AnimateAsync(nozzle, _targetPositionY, duration).Forget();
        }

        private async UniTask AnimateAsync(Nozzle nozzle, float positionY, float duration)
        {
            var targetPair = _nozzlePairs.First(t => t.Nozzle == nozzle);

            if (targetPair.Transform == null)
                return;

            _moveTween = targetPair.Transform.DOAnchorPosY(positionY, duration);

            ActiveNozzle = nozzle;

            _text.text = ActiveNozzle.ToString();

            await _moveTween.AsyncWaitForCompletion();
        }

        private void ActivateContent()
        {
            _content.SetActive(true);
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