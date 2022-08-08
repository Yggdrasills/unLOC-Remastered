﻿using DG.Tweening;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Workshop
{
    public class RoboCleanerView : MonoBehaviour
    {
        [SerializeField]
        private float _targetPositionX = -64f;

        [SerializeField]
        private float _moveDuration = 8f;

        [SerializeField]
        private float _awaitingDuration = 5f;

        private void Start()
        {
            Run();
        }

        private void Run()
        {
            Flip();

            DOTween.Sequence()
                .AppendCallback(Flip)
                .Append(transform.DOMoveX(_targetPositionX, _moveDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(_awaitingDuration)
                .AppendCallback(Flip)
                .Append(transform.DOMoveX(-_targetPositionX, _moveDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(_awaitingDuration)
                .SetLoops(-1, LoopType.Restart);
        }

        private void Flip()
        {
            var currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }
    }
}