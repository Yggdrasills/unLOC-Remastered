using System;

using SevenDays.unLOC.Services;

using UnityEngine;

namespace SevenDays.unLOC.Views
{
    public class ScrewdriverPickable : PickableBase
    {
        [SerializeField]
        private GameObject _screwdriverContent;

        public override InventoryItem Type => InventoryItem.Screwdriver;

        public override Action GetInventoryClickStrategy()
        {
            return () => _screwdriverContent.gameObject.SetActive(true);
        }
    }
}