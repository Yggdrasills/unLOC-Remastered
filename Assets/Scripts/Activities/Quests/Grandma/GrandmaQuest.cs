using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Grandma.Visualization;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Grandma
{
    public class GrandmaQuest : QuestBase
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private GlassesView _glassesPrefab;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private DraggableItemView[] _draggables;

        [SerializeField]
        private InteractableItem _grandmaView;

        private int _droppedAmount;

        private IInventoryService _inventory;
        private IStorageRepository _storage;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory,
            IStorageRepository storage)
        {
            _inventory = inventory;
            _storage = storage;
        }

        private void Start()
        {
            if (_storage.IsExists(typeof(GrandmaQuest).FullName))
            {
                Complete();
            }
        }

        public void StartQuest()
        {
            _camera.gameObject.SetActive(true);
            _content.SetActive(true);
        }

        private void OnEnable()
        {
            for (int i = 0; i < _draggables.Length; i++)
            {
                _draggables[i].Dropped += OnDropped;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _draggables.Length; i++)
            {
                _draggables[i].Dropped -= OnDropped;
            }
        }

        private void OnDropped()
        {
            _droppedAmount++;

            if (_droppedAmount >= _draggables.Length)
            {
                GiveGlassItem();

                Complete();
                _storage.Save(typeof(GrandmaQuest).FullName, true);
            }
        }

        private void Complete()
        {
            _grandmaView.Disable();

            gameObject.SetActive(false);
            _camera.gameObject.SetActive(false);

            // todo: start dialogue
            CompleteQuest();
        }

        private void GiveGlassItem()
        {
            var glasses = Instantiate(_glassesPrefab);
            _inventory.AddAsync(glasses).Forget();
        }
    }
}