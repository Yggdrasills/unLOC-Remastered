using UnityEngine;

namespace SevenDays.unLOC.Views
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        public Transform Container => _container;
    }
}