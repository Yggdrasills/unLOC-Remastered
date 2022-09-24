using System;
using System.Threading;

using AmayaSoft.Core.Utils.Extensions;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using DragonBones;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Player;

using UnityEngine;

using VContainer;

using Random = UnityEngine.Random;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    struct MechanoidAnimationKeys
    {
        public static readonly string Walk_Left = "Walk_Left";
        public static readonly string TMP_Sit_down = "TMP_Sit_down";
        public static readonly string Switch_off = "Switch_off";
        public static readonly string Standup = "Standup";
        public static readonly string Idle_1_Left = "Idle_1_Left";
        public static readonly string Idle_2_Left = "Idle_2_Left";
        public static readonly string Walk_Right = "Walk_Right";
        public static readonly string Idle_2_Right = "Idle_2_Right";
        public static readonly string Idle_1_Right = "Idle_1_Right";
        public static readonly string Talk_Left_tmp_2 = "Talk_Left_tmp_2";
        public static readonly string Talk_Left_tmp = "Talk_Left_tmp";
        public static readonly string Talk_Left = "Talk_Left";
    }

    public class MechanoidWalkingView : MonoBehaviour
    {
        [SerializeField]
        private float _speedMove = 12;

        [SerializeField]
        private UnityArmatureComponent _armatureComponent;

        [SerializeField]
        private float _playerRadius;

        [SerializeField]
        private Vector2 _timeRangeToSleep;

        [SerializeField]
        private float _playerCheckTime = 1;

        private PlayerView _followingPlayer;
        private CancellationTokenSource _sleepToken = new CancellationTokenSource();
        private int _currentDirection;
        private bool _isMove;

        private void Start()
        {
            StartMoveAsync().Forget();
        }


        [Inject]
        [UsedImplicitly]
        public void Inject(PlayerView followingPlayer)
        {
            _followingPlayer = followingPlayer;
        }

        private async UniTaskVoid StartMoveAsync()
        {
            while (gameObject)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_playerCheckTime));

                if (Mathf.Abs(transform.position.x - _followingPlayer.transform.position.x) < _playerRadius)
                {
                    if (_isMove)
                    {
                        IdleAsync().Forget();
                        _isMove = false;
                    }
                    continue;
                }

                _sleepToken?.Cancel();
                _sleepToken?.Dispose();

                _sleepToken = new CancellationTokenSource();

                var position = _followingPlayer.transform.position;
                var distance = position.x - transform.position.x;

                var direction = Math.Sign(distance);

                if (_currentDirection == direction && _isMove) continue;

                _currentDirection = direction;

                _armatureComponent.armature.flipX = _currentDirection == 1;

                _armatureComponent.animation.Play(_currentDirection > 0
                    ? MechanoidAnimationKeys.Walk_Right
                    : MechanoidAnimationKeys.Walk_Left);

                var targetPoint = position.x + _playerRadius * Random.Range(0, 2) * 2 - 1; // random -1 / 1 

                transform.DOMoveX(targetPoint, Mathf.Abs(distance) / _speedMove)
                    .SetEase(Ease.Linear);
                _isMove = true;
            }
        }

        private async UniTaskVoid IdleAsync()
        {
            _armatureComponent.animation.Play(_currentDirection > 0
                ? new[] { MechanoidAnimationKeys.Idle_1_Right, MechanoidAnimationKeys.Idle_2_Right }.GetRandomElement()
                : new[] { MechanoidAnimationKeys.Idle_1_Left, MechanoidAnimationKeys.Idle_2_Left }.GetRandomElement());

            // await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(_timeRangeToSleep.x, _timeRangeToSleep.x)),
            // cancellationToken: _sleepToken.Token);
        }
    }
}