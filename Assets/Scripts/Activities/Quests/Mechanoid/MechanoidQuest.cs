using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

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
        private ClickableItem _powerButton;

        [SerializeField]
        private ClickableItem _wires;

        [SerializeField]
        private ClickableItem _condenser;

        // todo: вынести работу со спрайтами в отдельный класс
        [SerializeField]
        private SpriteRenderer _powerButtonRenderer;

        [SerializeField]
        private SpriteRenderer _motherboardRenderer;

        [SerializeField]
        private SpriteRenderer _wiresRenderer;

        [SerializeField]
        private SpriteRenderer _condenserRenderer;

        [SerializeField]
        private SpriteRenderer _rivetRenderer;

        [SerializeField]
        private Sprite _burntMotherboardSprite;

        [SerializeField]
        private Sprite _wiresSprite;

        [SerializeField]
        private Sprite _burntCondenserSprite;

        [SerializeField]
        private Sprite _activePowerButtonSprite;

        [SerializeField]
        private Sprite _inactivePowerButtonSprite;

        private IInventoryService _inventoryService;

        [UsedImplicitly]
        [Inject]
        private void Construct(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        private void OnEnable()
        {
            _powerButton.Clicked += OnPowerClick;
            _wires.Clicked += OnWiresClick;
            _condenser.Clicked += OnCondenserClick;
        }

        private void OnDisable()
        {
            _powerButton.Clicked -= OnPowerClick;
            _wires.Clicked -= OnWiresClick;
            _condenser.Clicked -= OnCondenserClick;
        }

        private void OnPowerClick()
        {
            async UniTaskVoid OnClick()
            {
                if (_wiresRenderer.sprite != _wiresSprite)
                {
                    await _textDisplayer.DisplayTooEasySarcasm();
                }
                else
                {
                    // todo: display Loc text
                    _mechanoidView.PlayExplosion().Forget();
                    _motherboardRenderer.sprite = _burntMotherboardSprite;
                    _condenserRenderer.sprite = _burntCondenserSprite;
                    _rivetRenderer.enabled = false;
                    gameObject.SetActive(false);
                }
            }

            OnClick().Forget();
        }

        private void OnWiresClick()
        {
            async UniTaskVoid OnClick()
            {
                if (_inventoryService.Contains(InventoryItem.Wires))
                {
                    _inventoryService.Use(InventoryItem.Wires);

                    _wiresRenderer.sprite = _wiresSprite;
                    _powerButtonRenderer.sprite = _activePowerButtonSprite;
                    await _textDisplayer.DisplaySelfPraise();
                }
                else
                {
                    await _textDisplayer.DisplayForgotWires();
                }
            }

            OnClick().Forget();
        }

        private void OnCondenserClick()
        {
            async UniTaskVoid OnClick()
            {
            }

            OnClick().Forget();
        }
    }
}