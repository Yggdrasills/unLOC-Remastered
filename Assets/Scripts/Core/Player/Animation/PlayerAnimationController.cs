using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Player.Animations.Config;

using UnityEngine;

using Random = UnityEngine.Random;

namespace SevenDays.unLOC.Core.Player.Animations
{
    public class PlayerAnimationController :  IDisposable
    {
        private readonly PlayerView _playerView;
        private readonly Animator _playerAnimator;
        private readonly AnimationConfig _animationConfig;

        private CancellationTokenSource _specialIdleAwaitToken = new CancellationTokenSource();
        private bool _specIdleAwaiting;

        public PlayerAnimationController(PlayerView playerView)
        {
            _playerView = playerView;
            _animationConfig = playerView.AnimationConfig;
            _playerAnimator = playerView.PlayerAnimator;
        }
        
        public void Start()
        {
            _playerView.StartMove += OnPlayerStartMove;
            _playerView.Stay += OnPlayerStay;
            _playerView.AnimationCallBackView.IdleStart += OnIdleStart;
        }

        // review: диспоуз не вызывается
        void IDisposable.Dispose()
        {
            _specialIdleAwaitToken?.Cancel();
            _specialIdleAwaitToken?.Dispose();

            _playerView.StartMove -= OnPlayerStartMove;
            _playerView.Stay -= OnPlayerStay;
            _playerView.AnimationCallBackView.IdleStart -= OnIdleStart;
        }

        private void OnPlayerStay()
        {
            _playerAnimator.SetBool(_animationConfig.AnimatorWalkToggle, false);
        }

        private void OnIdleStart()
        {
            // review: вместо булевой можно кешировать UniTask AwaitEnableSpecialStayAsync (вместо UniTaskVoid)
            // review: и проверить myTask.Status.IsCompleted();
            if (_specIdleAwaiting) return;

            _specialIdleAwaitToken.Cancel();
            
            // review: не диспоузится перед пересозданием
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
            // review: судя по коду может отмениться дважды. Если отменяется уже отмененный то выпадет ошибка
            _specialIdleAwaitToken.Cancel();
        }
    }
}