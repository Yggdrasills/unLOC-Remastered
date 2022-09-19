using JetBrains.Annotations;

using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items
{
    public class CanvasCameraSetter : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        private Camera _mainCamera;

        [Inject, UsedImplicitly]
        private void Construct(IStorageRepository storage, Camera mainCamera)
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