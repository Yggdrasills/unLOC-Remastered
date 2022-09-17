using JetBrains.Annotations;

using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items.Pad
{
    public class PadView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _content;

        private DataStorage _storage;

        [Inject, UsedImplicitly]
        private void Construct(DataStorage storage)
        {
            _storage = storage;
        }

        private void Awake()
        {
            if (_storage.IsExists(typeof(PadItem).FullName))
            {
                _content.SetActive(true);
            }
        }

        public void PickUp()
        {
            _content.SetActive(true);
            _storage.Save(typeof(PadView).FullName, true);
        }
    }
}