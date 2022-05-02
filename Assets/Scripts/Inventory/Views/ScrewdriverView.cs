using System;

using SevenDays.unLOC.Services;

namespace SevenDays.unLOC.Views
{
    public class ScrewdriverView : PickableBaseView
    {
        public override InventoryItem Type => InventoryItem.Screwdriver;

        public override Action GetInventoryClickStrategy()
        {
            return base.GetInventoryClickStrategy();
        }
    }
}