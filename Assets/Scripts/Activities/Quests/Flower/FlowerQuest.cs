using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Flower.Screwdriver;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerQuest : QuestBase
    {
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

        private ScrewdriverView _screwdriver;

        private List<ClickableItem> _brokenPlugList;
        private List<EmptyPlugView> _emptyPlugList;

        [Inject, UsedImplicitly]
        private void Construct(IInventoryService inventory, ScrewdriverView screwdriverView)
        {
            _inventory = inventory;
            _screwdriver = screwdriverView;
        }

        private void Awake()
        {
            _brokenPlugList = new List<ClickableItem>(_brokenPlugs);
            _emptyPlugList = new List<EmptyPlugView>(_emptyPlugs);

            _brokenPlugList.ForEach(cI => cI.Clicked += () => OnBrokenPlugClicked(cI));
            _emptyPlugList.ForEach(cI => cI.Clicked += () => OnEmptyPlugClicked(cI));

            _flowerView.Clicked += ActivateSelf;
        }

        private void OnDestroy()
        {
            _flowerView.Clicked -= ActivateSelf;
        }

        private void ActivateSelf()
        {
            _content.SetActive(true);
        }

        private void OnBrokenPlugClicked(ClickableItem plug)
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

            _brokenPlugList.Remove(plug);

            plug.gameObject.SetActive(false);

            if (!_brokenPlugList.Any())
            {
                _inventory.Use(InventoryItem.Screwdriver);

                _textDisplayer.DisplayScrewsDescription();

                _firstStage.SetActive(false);
                _secondStage.SetActive(true);

                FirstStagePassed = true;
            }
        }

        private void OnEmptyPlugClicked(EmptyPlugView plug)
        {
            if (_inventory.Contains(InventoryItem.ScrewSpanner))
            {
                _inventory.Use(InventoryItem.ScrewSpanner);

                plug.ActivateBolt();

                _emptyPlugList.Remove(plug);

                if (_emptyPlugList.Any())
                    return;

                CompleteQuest();
                _flowerView.SetFlowerRepairedSprite();
                gameObject.SetActive(false);

                _inventory.Remove(InventoryItem.ScrewEdge3);
                _inventory.Remove(InventoryItem.ScrewRadiation);
            }

            else if (_inventory.Contains(InventoryItem.ScrewEdge3) ||
                     _inventory.Contains(InventoryItem.ScrewRadiation))
            {
                _textDisplayer.DisplayIncorrectBolt();
            }
        }
    }
}