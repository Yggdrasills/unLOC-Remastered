using JetBrains.Annotations;

using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items.Pad
{
    public class PadView : MonoBehaviour
    {
        [SerializeField]
        private Canvas[] _canvases;

        [SerializeField]
        private GameObject _content;

        private IStorageRepository _storage;
        private Camera _mainCamera;

        [Inject, UsedImplicitly]
        private void Construct(IStorageRepository storage, Camera mainCamera)
        {
            _storage = storage;
            _mainCamera = mainCamera;
        }

        private void OnValidate()
        {
            if (_canvases == null || _canvases.Length < 1)
            {
                _canvases = GetComponentsInChildren<Canvas>();
            }
        }

        private void Start()
        {
            if (_storage.IsExists(typeof(PadItem).FullName))
            {
                _content.SetActive(true);
            }

            for (int i = 0; i < _canvases.Length; i++)
            {
                _canvases[i].worldCamera = _mainCamera;
            }
        }

        public void PickUp()
        {
            _content.SetActive(true);
            _storage.Save(typeof(PadView).FullName, true);
        }
    }
}