using System.Collections.Generic;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Flower.Screwdriver;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerQuest : QuestBase
    {
        private static readonly string BrokenPlugKey = typeof(FlowerQuest).FullName + nameof(_brokenPlugIndexes);
        private static readonly string EmptyPlugKey = typeof(FlowerQuest).FullName + nameof(_emptyPlugIndexes);

        public bool FirstStagePassed { get; private set; }

        [SerializeField]
        private FlowerView _flowerView;

        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private FlowerQuestTextDisplayer _textDisplayer;

        [SerializeField]
        private ClickableItem[] _brokenPlugs;

        [SerializeField]
        private EmptyPlugView[] _emptyPlugs;

        [SerializeField]
        private GameObject _firstStage;

        [SerializeField]
        private GameObject _secondStage;

        [SerializeField]
        private Nozzle _targetNozzle;

        private IInventoryService _inventory;

        private DataStorage _storage;

        private ScrewdriverView _screwdriver;

        private List<int> _brokenPlugIndexes;
        private List<int> _emptyPlugIndexes;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory,
            DataStorage storage,
            ScrewdriverView screwdriverView)
        {
            _inventory = inventory;
            _screwdriver = screwdriverView;
            _storage = storage;
        }

        private void Awake()
        {
            if (!_storage.TryLoad(BrokenPlugKey, out _brokenPlugIndexes))
            {
                _brokenPlugIndexes = new List<int>(_brokenPlugs.Length);
            }
            else
            {
                for (int i = 0, k = _brokenPlugIndexes.Count; i < k; i++)
                {
                    RemoveBrokenPlug(_brokenPlugIndexes[i]);
                }
            }

            if (!_storage.TryLoad(EmptyPlugKey, out _emptyPlugIndexes))
            {
                _emptyPlugIndexes = new List<int>(_emptyPlugs.Length);
            }
            else
            {
                for (int i = 0, k = _emptyPlugIndexes.Count; i < k; i++)
                {
                    ReplaceEmptyPlug(_brokenPlugIndexes[i]);
                }
            }

            for (int i = 0; i < _brokenPlugs.Length; i++)
            {
                var closure = i;
                _brokenPlugs[i].Clicked += () => OnBrokenPlugClicked(closure);
            }

            for (int i = 0; i < _emptyPlugs.Length; i++)
            {
                var closure = i;
                _emptyPlugs[i].Clicked += () => OnEmptyPlugClicked(closure);
            }

            _flowerView.Clicked += ActivateSelf;
        }

        private void OnDestroy()
        {
            _storage.Save(BrokenPlugKey, _brokenPlugIndexes);
            _storage.Save(EmptyPlugKey, _emptyPlugIndexes);

            _flowerView.Clicked -= ActivateSelf;
        }

        private void ActivateSelf()
        {
            _content.SetActive(true);
        }

        private void OnBrokenPlugClicked(int brokenPlugIndex)
        {
            if (!_inventory.Contains(InventoryItem.Screwdriver))
            {
                _textDisplayer.DisplayScrewdriverNotFound();

                return;
            }

            if (_screwdriver.ActiveNozzle != _targetNozzle)
            {
                _textDisplayer.DisplayIncorrectNozzle();

                return;
            }

            _brokenPlugIndexes.Add(brokenPlugIndex);

            RemoveBrokenPlug(brokenPlugIndex);
        }

        private void RemoveBrokenPlug(int brokenPlugIndex)
        {
            _brokenPlugs[brokenPlugIndex].gameObject.SetActive(false);

            if (_brokenPlugIndexes.Count >= _brokenPlugs.Length)
            {
                if (_inventory.Contains(InventoryItem.Screwdriver))
                {
                    _inventory.Use(InventoryItem.Screwdriver);
                }

                _textDisplayer.DisplayScrewsDescription();

                _firstStage.SetActive(false);
                _secondStage.SetActive(true);

                FirstStagePassed = true;
            }
        }

        private void OnEmptyPlugClicked(int emptyPlugIndex)
        {
            if (_inventory.Contains(InventoryItem.ScrewSpanner))
            {
                _inventory.Use(InventoryItem.ScrewSpanner);

                _emptyPlugIndexes.Add(emptyPlugIndex);

                ReplaceEmptyPlug(emptyPlugIndex);
            }

            else if (_inventory.Contains(InventoryItem.ScrewEdge3) ||
                     _inventory.Contains(InventoryItem.ScrewRadiation))
            {
                _textDisplayer.DisplayIncorrectBolt();
            }
        }

        private void ReplaceEmptyPlug(int emptyPlugIndex)
        {
            _emptyPlugs[emptyPlugIndex].ActivateBolt();

            if (_emptyPlugIndexes.Count >= _emptyPlugs.Length)
            {
                CompleteQuest();
                _flowerView.SetFlowerRepairedSprite();
                gameObject.SetActive(false);

                _inventory.Remove(InventoryItem.ScrewEdge3);
                _inventory.Remove(InventoryItem.ScrewRadiation);
            }
        }
    }
}