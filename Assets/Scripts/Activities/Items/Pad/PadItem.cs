using JetBrains.Annotations;

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

        private DataStorage _storage;

        [Inject, UsedImplicitly]
        private void Constructor(PadView padView, DataStorage storage)
        {
            _padView = padView;
            _storage = storage;
        }

        private void Awake()
        {
            if (_storage.IsExists(typeof(PadItem).FullName))
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

            _storage.Save(typeof(PadItem).FullName, true);
        }
    }
}