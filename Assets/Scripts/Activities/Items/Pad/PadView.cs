using SevenDays.unLOC.Storage;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Items.Pad
{
    public class PadView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _content;

        private DataStorage _dataStorage;

        private void Awake()
        {
            _dataStorage = new DataStorage();

            if (_dataStorage.IsExists(typeof(PadItem).FullName))
            {
                _content.SetActive(true);
            }
        }

        public void PickUp()
        {
            _content.SetActive(true);
            _dataStorage.Save(typeof(PadView).FullName, true);
        }
    }
}