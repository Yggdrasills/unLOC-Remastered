using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Animations.Config;
using SevenDays.unLOC.Core.Movement;

using UnityEngine;

using VContainer.Unity;

using Random = UnityEngine.Random;

namespace SevenDays.unLOC.Core.Animations
{
    public class PlayerAnimationController : IInitializable
    {
        private readonly PlayerView _playerView;
        private readonly Animator _playerAnimator;
        private readonly AnimationConfig _animationConfig;

        private CancellationTokenSource _specialIdleAwaitToken = new CancellationTokenSource();
        private bool _specIdleAwaiting = false;

        public PlayerAnimationController(PlayerView playerView, AnimationConfig animationConfig)
        {
            _playerView = playerView;
            _animationConfig = animationConfig;
            _playerAnimator = playerView.PlayerAnimator;
        }

        void IInitializable.Initialize()
        {
            _playerView.StartMove += OnPlayerStartMove;
            _playerView.Stay += OnPlayerStay;
            _playerView.AnimationCallBackView.IdleStart += OnIdleStart;
        }

        private void OnPlayerStay()
        {
            _playerAnimator.SetBool(_animationConfig.AnimatorWalkToggle, false);
        }

        private void OnIdleStart()
        {
            if (_specIdleAwaiting) return;

            _specialIdleAwaitToken.Cancel();
            _specialIdleAwaitToken = new CancellationTokenSource();
            AwaitEnableSpecialStayAsync(_specialIdleAwaitToken.Token).Forget();
        }

        private async UniTaskVoid AwaitEnableSpecialStayAsync(CancellationToken cancellationToken)
        {
            _specIdleAwaiting = true;
            cancellationToken.Register(() => _specIdleAwaiting = false);

            var range = _animationConfig.RangeRandomEnabledSpecIdle;

            await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(range.x, range.y + 1)),
                cancellationToken: cancellationToken);

            _specIdleAwaiting = false;

            _playerAnimator.SetTrigger(_animationConfig.AnimatorSpecIdleTriggers[
                Random.Range(0, _animationConfig.AnimatorSpecIdleTriggers.Length)]);
        }


        private void OnPlayerStartMove()
        {
            _playerAnimator.SetBool(_animationConfig.AnimatorWalkToggle, true);
            _specialIdleAwaitToken.Cancel();
        }
    }
}