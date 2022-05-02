using SevenDays.unLOC.Services;
using SevenDays.unLOC.Views;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core
{
    public class WorkshopCompositeRoot : LifetimeScope
    {
        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryView;

        [SerializeField]
        private PickableBaseView[] _pickables;

        protected override void Configure(IContainerBuilder builder)
        {
            // todo: replace by RegisterEntryPoint
            var inventoryService = new InventoryService(_cellPrefab, _inventoryView);

            foreach (var pickable in _pickables)
            {
                inventoryService.HandlePickable(pickable);
            }
        }
    }
}