using UnityEngine;

namespace SevenDays.unLOC.Inventory.Views
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        public Transform Container => _container;
    }
}