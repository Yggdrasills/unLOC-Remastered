using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Quests.Printer;
using SevenDays.unLOC.Inventory.Services;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    /// <summary>
    /// note: Для теста квеста mechanoid
    /// </summary>
    public class WiresViewDummy : WiresView
    {
        private IInventoryService _inventoryService;

        [UsedImplicitly]
        [Inject]
        private void Construct(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Pick()
        {
            _inventoryService.AddAsync(this).Forget();
        }
    }
}