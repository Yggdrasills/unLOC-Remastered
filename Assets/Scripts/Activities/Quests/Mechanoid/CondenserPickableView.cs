using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine.EventSystems;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class CondenserPickableView : PickableBase, IPointerClickHandler
    {
        public override InventoryItem Type => InventoryItem.Condenser;


        private IInventoryService _inventory;

        [Inject]
        [UsedImplicitly]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventory.AddAsync(this).Forget();
        }
    }
}