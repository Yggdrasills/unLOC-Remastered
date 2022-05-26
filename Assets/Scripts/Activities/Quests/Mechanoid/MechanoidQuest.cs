using System;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidQuest : QuestBase
    {
        private enum Stages
        {
            None,
            Wires,
            PowerButton,
            PullOffCondenser,
            SetCondenser,
            RunMechanoid
        }

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

        private Stages _activeStage = Stages.Wires;

        private IInventoryService _inventoryService;

        [UsedImplicitly]
        [Inject]
        private void Construct(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        private void OnEnable()
        {
            _powerButtonView.Clicked += OnPowerClick;
            _wiresView.Clicked += OnWiresClick;
            _condenserView.Clicked += OnCondenserClick;

            _textDisplayer.ResetToDefault();
        }

        private void OnDisable()
        {
            _powerButtonView.Clicked -= OnPowerClick;
            _wiresView.Clicked -= OnWiresClick;
            _condenserView.Clicked -= OnCondenserClick;
        }

        public bool IsLastStage()
        {
            return _activeStage == Stages.SetCondenser;
        }

        private void OnPowerClick()
        {
            async UniTaskVoid OnClick()
            {
                if (_activeStage == Stages.PowerButton)
                {
                    _motherBoardView.SetSprite();
                    _rivetView.SetSprite();

                    _powerButtonView.ResetToDefault();

                    gameObject.SetActive(false);

                    await _mechanoidView.PlayExplosion();

                    // todo: await Loc replica

                    gameObject.SetActive(true);

                    _activeStage = Stages.PullOffCondenser;
                }
                else if (_activeStage == Stages.Wires)
                {
                    _textDisplayer.DisplayTooEasySarcasm().Forget();
                }
                else if (_activeStage == Stages.RunMechanoid)
                {
                    // note: visual delay
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                    gameObject.SetActive(false);

                    // todo: enable mechanoid
                }
                else
                {
                    _textDisplayer.DisplayCantTurnOn().Forget();
                }
            }

            OnClick().Forget();
        }

        private void OnWiresClick()
        {
            if (_activeStage == Stages.Wires)
            {
                if (_inventoryService.Contains(InventoryItem.Wires))
                {
                    _inventoryService.Use(InventoryItem.Wires);

                    _wiresView.SetSprite();
                    _powerButtonView.SetSprite();

                    _textDisplayer.DisplaySelfPraise().Forget();

                    _activeStage = Stages.PowerButton;
                }
                else
                {
                    _textDisplayer.DisplayForgotWires().Forget();
                }
            }
            else
            {
                _textDisplayer.DisplayWiresOnPlace().Forget();
            }
        }

        private void OnCondenserClick()
        {
            if (_activeStage == Stages.PullOffCondenser)
            {
                _rivetView.gameObject.SetActive(false);

                _activeStage = Stages.SetCondenser;
            }

            else if (_activeStage == Stages.SetCondenser)
            {
                if (_inventoryService.Contains(InventoryItem.Condenser))
                {
                    _inventoryService.Use(InventoryItem.Condenser);
                    _condenserView.SetSprite();

                    _activeStage = Stages.RunMechanoid;
                    _textDisplayer.DisplayTimeToTurnOn().Forget();
                }
                else
                {
                    _textDisplayer.DisplayNoCondenser().Forget();
                }
            }
        }
    }
}