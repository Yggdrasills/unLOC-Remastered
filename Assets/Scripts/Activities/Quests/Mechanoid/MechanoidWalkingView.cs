using System;
using System.Threading;
using System.Threading.Tasks;

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
    public class MechanoidWalkingView : MonoBehaviour
    {
        private const float CloserValue = .1f;
        private const float StartAnimationMultipier = .7f;

        [SerializeField]
        private float _speedMove = 12;

        [SerializeField]
        private UnityArmatureComponent _armatureComponent;

        [SerializeField]
        private float _playerRadius;

        [SerializeField]
        private Vector2 _timeRangeToBorring = new Vector2(6, 9);

        [SerializeField]
        private Vector2 _timeRangeMoveWhenBorring = new Vector2(2f, 3f);

        [SerializeField]
        private float _playerCheckTime = 1;

        [SerializeField]
        private Vector3 _jumpPosition = new Vector3(-25, -3.6f, 0);

        [SerializeField]
        private float _fadeInAnimationTime = .5f;

        private CancellationTokenSource _secondIdleToken = new CancellationTokenSource();

        private PlayerView _followingPlayer;

        private int _currentDirection;

        private float _playerTargetOffset;

        private RobotState _state;

        private float _timeCheckPlayerMove;

        private float _timeToBoring;

        private float _currentTimeInStay;

        private float _timeToSecondIdleAnimation;

        [Inject]
        [UsedImplicitly]
        public void Inject(PlayerView followingPlayer)
        {
            _followingPlayer = followingPlayer;
        }

        private void Update()
        {
            if (_followingPlayer is null)
            {
                return;
            }

            switch (_state)
            {
                case RobotState.MoveToPlayer:
                    OnMove();
                    break;
                case RobotState.Stay:
                    OnStay();
                    break;
                case RobotState.Enabling:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _armatureComponent.armature.flipX = _currentDirection == 1;
        }


        public async UniTaskVoid EnableAsync()
        {
            _state = RobotState.Enabling;

            _armatureComponent.animation.FadeIn(MechanoidAnimationKeys.Standup);
            await UniTask.Yield();
            await UniTask.Delay(TimeSpan.FromSeconds(_armatureComponent.animation.lastAnimationState._duration * StartAnimationMultipier));

            _armatureComponent.animation.FadeIn(MechanoidAnimationKeys.Idle_2_Left, _fadeInAnimationTime);
            await UniTask.Yield();
            await UniTask.Delay(TimeSpan.FromSeconds(_armatureComponent.animation.lastAnimationState._duration * StartAnimationMultipier));
            
            await transform.DOJump(_jumpPosition, 6, 1, .5f);

            SetStateMove();
            _timeToBoring = Random.Range(_timeRangeToBorring.x, _timeRangeToBorring.y);
        }

        private void OnMove()
        {
            var distance = GetDistance();
            var direction = GetDistanceDirection(distance);

            if (direction != _currentDirection)
            {
                SetWalkAnimation();
            }

            _currentDirection = direction;

            if (Mathf.Abs(distance) <= CloserValue)
            {
                SetStateStay();
                return;
            }

            transform.Translate(Vector2.right * _currentDirection * _speedMove * Time.deltaTime);
        }

        private void OnStay()
        {
            _timeCheckPlayerMove += Time.deltaTime;
            _currentTimeInStay += Time.deltaTime;

            if (_currentTimeInStay > _timeToSecondIdleAnimation)
            {
                SetSecondIdleAnimationAsync().Forget();
            }

            if (_currentTimeInStay > _timeToBoring)
            {
                SetStateMove();
                _timeToBoring = Random.Range(_timeRangeMoveWhenBorring.x, _timeRangeMoveWhenBorring.y);
                _currentTimeInStay = 0;
            }

            if (_timeCheckPlayerMove < _playerCheckTime)
            {
                return;
            }

            _timeCheckPlayerMove = 0;
            var distanceToPlayer = _followingPlayer.transform.position.x - transform.position.x;

            if (Mathf.Abs(distanceToPlayer) > _playerRadius)
            {
                SetStateMove();
                _timeToBoring = Random.Range(_timeRangeToBorring.x, _timeRangeToBorring.y);
                _currentTimeInStay = 0;
            }
        }

        private int GetDistanceDirection(float distance)
        {
            var direction = Math.Sign(distance);
            return direction;
        }

        private float GetDistance()
        {
            var positionX = _followingPlayer.transform.position.x + _playerTargetOffset;
            return positionX - transform.position.x;
        }

        private void SetStateStay()
        {
            _state = RobotState.Stay;
            _timeToSecondIdleAnimation = Random.Range(_timeRangeMoveWhenBorring.y, _timeRangeToBorring.x);

            SetIdleAnimation();
        }

        private void SetStateMove()
        {
            _state = RobotState.MoveToPlayer;
            _playerTargetOffset = PickRandomOffset();

            if (!_secondIdleToken.IsCancellationRequested)
            {
                _secondIdleToken?.Cancel();
                _secondIdleToken?.Dispose();
            }

            SetWalkAnimation();
        }

        private void SetIdleAnimation()
        {
            var animationName = _currentDirection == 1
                ? MechanoidAnimationKeys.Idle_1_Right
                : MechanoidAnimationKeys.Idle_1_Left;
            _armatureComponent.animation.FadeIn(animationName, _fadeInAnimationTime);
        }

        private async UniTaskVoid SetSecondIdleAnimationAsync()
        {
            var animationName = _currentDirection == 1
                ? MechanoidAnimationKeys.Idle_2_Right
                : MechanoidAnimationKeys.Idle_2_Left;
            if (_armatureComponent.animation.lastAnimationName == animationName)
            {
                return;
            }

            _armatureComponent.animation.FadeIn(animationName, _fadeInAnimationTime);

            _secondIdleToken = new CancellationTokenSource();
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_armatureComponent.animation.lastAnimationState._duration),
                    cancellationToken: _secondIdleToken.Token);
                SetIdleAnimation();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SetWalkAnimation()
        {
            var previousDirection = _currentDirection;
            _currentDirection = GetDistanceDirection(GetDistance());
            var animationName = _currentDirection == 1
                ? MechanoidAnimationKeys.Walk_Right
                : MechanoidAnimationKeys.Walk_Left;

            _armatureComponent.animation.FadeIn(animationName,
                previousDirection == _currentDirection ? _fadeInAnimationTime : 0);
        }

        private float PickRandomOffset()
        {
            return Random.Range(-_playerRadius, _playerRadius);
        }
    }
}