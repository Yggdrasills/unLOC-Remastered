using SevenDays.unLOC.Activities.Items.Pad;
using SevenDays.unLOC.Activities.Quests.Flower.Screwdriver;
using SevenDays.unLOC.Inventory.Views;

using UnityEngine;

namespace SevenDays.unLOC.Core.Loaders
{
    [CreateAssetMenu(menuName = "SevenDays/unLOC/" + nameof(GameServiceData))]
    public class GameServiceData : ScriptableObject
    {
        public InventoryCellView CellPrefab => _cellPrefab;

        public InventoryView InventoryViewPrefab => _inventoryViewPrefab;

        public PadView PadViewPrefab => _padViewPrefab;

        public ScrewdriverView ScrewdriverViewPrefab => _screwdriverViewPrefab;

        [SerializeField]
        private InventoryCellView _cellPrefab;

        [SerializeField]
        private InventoryView _inventoryViewPrefab;

        [SerializeField]
        private PadView _padViewPrefab;

        [SerializeField]
        private ScrewdriverView _screwdriverViewPrefab;
    }
}