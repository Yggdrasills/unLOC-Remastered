using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Workshop.Views
{
    [RequireComponent(typeof(InteractableItem))]
    public class NoteView : QuestBase
    {
        [SerializeField]
        private InteractableItem _interactableItem;

        private DataStorage _storage;

        [Inject, UsedImplicitly]
        private void Construct(DataStorage storage)
        {
            _storage = storage;
        }

        private void OnValidate()
        {
            if (_interactableItem == null)
            {
                _interactableItem = GetComponent<InteractableItem>();
            }
        }

        private void Start()
        {
            if (_storage.IsExists(typeof(NoteView).FullName))
            {
                gameObject.SetActive(false);

                return;
            }

            _interactableItem.Clicked += OnClick;
        }

        private void OnDestroy()
        {
            _interactableItem.Clicked -= OnClick;
        }

        private void OnClick()
        {
            CompleteQuest();

            gameObject.SetActive(false);

            _storage.Save(typeof(NoteView).FullName, true);
        }
    }
}