using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Printer
{
    public class BottleView : PickableBase
    {
        [SerializeField]
        private InteractableItem _clickableItem;

        public override InventoryItem Type => InventoryItem.Bottle;

        private IInventoryService _inventory;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        private void OnValidate()
        {
            _clickableItem ??= GetComponent<InteractableItem>();
        }

        private void OnEnable()
        {
            _clickableItem.Clicked += OnClick;
        }

        private void OnDisable()
        {
            _clickableItem.Clicked -= OnClick;
        }

        private void OnClick()
        {
            _inventory.AddAsync(this).Forget();
        }
    }
}