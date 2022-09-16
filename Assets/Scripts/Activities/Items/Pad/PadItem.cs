using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items.Pad
{
    [RequireComponent(typeof(InteractableItem))]
    public class PadItem : MonoBehaviour
    {
        [SerializeField]
        private InteractableItem _interactableItem;

        private PadView _padView;

        private DataStorage _dataStorage;

        [Inject]
        private void Constructor(PadView padView)
        {
            _padView = padView;
        }

        private void Awake()
        {
            _dataStorage = new DataStorage();

            if (_dataStorage.IsExists(typeof(PadItem).FullName))
            {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _interactableItem.Clicked += OnClicked;
        }

        private void OnDestroy()
        {
            _interactableItem.Clicked -= OnClicked;
        }

        private void OnValidate()
        {
            if (_interactableItem is null)
            {
                _interactableItem = GetComponent<InteractableItem>();
            }
        }

        private void OnClicked()
        {
            gameObject.SetActive(false);
            _padView.PickUp();

            _dataStorage.Save(typeof(PadItem).FullName, true);
        }
    }
}