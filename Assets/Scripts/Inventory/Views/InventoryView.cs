using JetBrains.Annotations;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Inventory.Views
{
    public class InventoryView : MonoBehaviour
    {
        public Transform Container => _container;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Transform _container;

        private Camera _mainCamera;

        [Inject, UsedImplicitly]
        private void Construct(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        private void OnValidate()
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
        }

        private void Start()
        {
            _canvas.worldCamera = _mainCamera;
        }
    }
}