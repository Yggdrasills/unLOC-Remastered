using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player.Animations.Config;

using UnityEngine;

using Random = UnityEngine.Random;

namespace SevenDays.unLOC.Core.Player.Animations
{
    public class PlayerAnimationController : IInitialize, IDisposable
    {
        private readonly PlayerView _playerView;
        private readonly Animator _playerAnimator;
        private readonly AnimationConfig _animationConfig;
        private readonly IMovementModel _movementModel;

        private CancellationTokenSource _specialIdleAwaitToken = new CancellationTokenSource();
        private bool _specIdleAwaiting;

        public PlayerAnimationController(IMovementModel playerMovement, PlayerView playerView)
        {
            _playerView = playerView;
            _animationConfig = playerView.AnimationConfig;
            _playerAnimator = playerView.PlayerAnimator;
            _movementModel = playerMovement;
        }

        public void Initialize()
        {
            _movementModel.StartMove += OnPlayerStartMove;
            _movementModel.StopMove += OnPlayerStay;
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
            _specialIdleAwaitToken.Dispose();
            _specialIdleAwaitToken = new CancellationTokenSource();
            AwaitEnableSpecialStayAsync(_specialIdleAwaitToken.Token).Forget();
        }

        private async UniTaskVoid AwaitEnableSpecialStayAsync(CancellationToken cancellationToken)
        {
            _specIdleAwaiting = true;

            var range = _animationConfig.RangeRandomEnabledSpecIdle;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(range.x, range.y + 1)),
                    cancellationToken: cancellationToken);
                _specIdleAwaiting = false;

                _playerAnimator.SetTrigger(_animationConfig.AnimatorSpecIdleTriggers[
                    Random.Range(0, _animationConfig.AnimatorSpecIdleTriggers.Length)]);
            }
            catch (Exception)
            {
                _specIdleAwaiting = false;
            }

        }


        private void OnPlayerStartMove()
        {
            _playerAnimator.SetBool(_animationConfig.AnimatorWalkToggle, true);
            _specialIdleAwaitToken.Cancel();
        }

        void IDisposable.Dispose()
        {
            _specialIdleAwaitToken?.Cancel();
            _specialIdleAwaitToken?.Dispose();

            _movementModel.StartMove -= OnPlayerStartMove;
            _movementModel.StopMove -= OnPlayerStay;
            _playerView.AnimationCallBackView.IdleStart -= OnIdleStart;
        }
    }
}