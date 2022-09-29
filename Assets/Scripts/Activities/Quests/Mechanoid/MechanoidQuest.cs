using System;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidQuest : QuestBase
    {
        [SerializeField]
        private MechanoidTextDisplayer _textDisplayer;

        [SerializeField]
        private MechanoidView _mechanoidView;

        [SerializeField]
        private MechanoidItemView _powerButtonView;

        [SerializeField]
        private MechanoidItemView _wiresView;

        [SerializeField]
        private MechanoidItemView _condenserView;

        [SerializeField]
        private MechanoidItemView _motherBoardView;

        [SerializeField]
        private MechanoidItemView _rivetView;

        private State _activeState = State.Wires;

        private IInventoryService _inventoryService;
        private IStorageRepository _storage;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventoryService,
            IStorageRepository storage)
        {
            _inventoryService = inventoryService;
            _storage = storage;
        }

        private void Start()
        {
            if (_storage.TryLoad(typeof(MechanoidQuest).FullName, out State state))
            {
                _activeState = state;

                switch (_activeState)
                {
                    case State.RunMechanoid:
                        gameObject.SetActive(false);
                        _mechanoidView.gameObject.SetActive(false);
                        CompleteQuest();
                        break;
                    case State.SetCondenser:
                        SetPullOffCondenserState();
                        SetPowerButtonState();
                        SetWiresState();
                        break;
                    case State.PullOffCondenser:
                        SetPowerButtonState();
                        SetWiresState();
                        break;
                    case State.PowerButton:
                        SetWiresState();
                        break;
                }
            }

            _powerButtonView.Clicked += OnPowerClick;
            _wiresView.Clicked += OnWiresClick;
            _condenserView.Clicked += OnCondenserClick;
        }

        private void OnDestroy()
        {
            _storage.Save(typeof(MechanoidQuest).FullName, _activeState);

            _powerButtonView.Clicked -= OnPowerClick;
            _wiresView.Clicked -= OnWiresClick;
            _condenserView.Clicked -= OnCondenserClick;
        }

        public bool IsLastStage()
        {
            return _activeState == State.SetCondenser;
        }

        private void OnWiresClick()
        {
            if (_activeState == State.Wires)
            {
                if (_inventoryService.Contains(InventoryItem.Wires))
                {
                    _inventoryService.Use(InventoryItem.Wires);

                    SetWiresState();

                    _textDisplayer.DisplaySelfPraise();

                    _activeState = State.PowerButton;
                }
                else
                {
                    _textDisplayer.DisplayForgotWires();
                }
            }
            else
            {
                _textDisplayer.DisplayWiresOnPlace();
            }
        }

        private void OnPowerClick()
        {
            async UniTaskVoid OnClickAsync()
            {
                if (_activeState == State.PowerButton)
                {
                    SetPowerButtonState();

                    gameObject.SetActive(false);

                    await _mechanoidView.PlayExplosionAsync();

                    // todo: await Loc replica

                    gameObject.SetActive(true);

                    _activeState = State.PullOffCondenser;
                }
                else if (_activeState == State.Wires)
                {
                    _textDisplayer.DisplayTooEasySarcasm();
                }
                else if (_activeState == State.RunMechanoid)
                {
                    // note: visual delay
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

                    gameObject.SetActive(false);
                    _mechanoidView.Disable();

                    CompleteQuest();

                    // todo: enable mechanoid
                }
                else
                {
                    _textDisplayer.DisplayCantTurnOn();
                }
            }

            OnClickAsync().Forget();
        }

        private void OnCondenserClick()
        {
            if (_activeState == State.PullOffCondenser)
            {
                SetPullOffCondenserState();

                _activeState = State.SetCondenser;
            }

            else if (_activeState == State.SetCondenser)
            {
                if (_inventoryService.Contains(InventoryItem.Condenser))
                {
                    _inventoryService.Use(InventoryItem.Condenser);
                    SetCondenserState();

                    _activeState = State.RunMechanoid;
                    _textDisplayer.DisplayTimeToTurnOn();
                }
                else
                {
                    _textDisplayer.DisplayNoCondenser();
                }
            }
        }

        private void SetCondenserState()
        {
            _condenserView.SetSprite();
        }

        private void SetPullOffCondenserState()
        {
            _rivetView.gameObject.SetActive(false);
        }

        private void SetPowerButtonState()
        {
            _motherBoardView.SetSprite();
            _rivetView.SetSprite();

            _powerButtonView.ResetToDefault();
        }

        private void SetWiresState()
        {
            _wiresView.SetSprite();
            _powerButtonView.SetSprite();
        }

        private enum State
        {
            Wires,
            PowerButton,
            PullOffCondenser,
            SetCondenser,
            RunMechanoid
        }
    }
}