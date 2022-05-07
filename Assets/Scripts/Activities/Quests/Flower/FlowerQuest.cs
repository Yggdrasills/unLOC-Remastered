using System.Collections.Generic;
using System.Linq;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests.Flower.Screwdriver;
using SevenDays.unLOC.Inventory;
using SevenDays.unLOC.Inventory.Services;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerQuest : MonoBehaviour
    {
        public bool FirstStagePassed { get; private set; }

        [SerializeField]
        private FlowerQuestTextDisplayer _textDisplayer;

        [SerializeField]
        private ScrewdriverView _screwdriver;

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

        private List<GameObject> _brokenPlugList;
        private List<EmptyPlugView> _emptyPlugList;

        [Inject]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        private void Awake()
        {
            _brokenPlugList = new List<GameObject>(_brokenPlugs.Length);
            _emptyPlugList = new List<EmptyPlugView>(_emptyPlugs.Length);

            for (int i = 0; i < _brokenPlugs.Length; i++)
            {
                int closure = i;

                _brokenPlugList.Add(_brokenPlugs[i].gameObject);

                _brokenPlugs[i].Clicked += () => OnBrokenPlugClicked(_brokenPlugs[closure].gameObject);
            }

            for (int i = 0; i < _emptyPlugs.Length; i++)
            {
                int closure = i;

                _emptyPlugList.Add(_emptyPlugs[i]);

                _emptyPlugs[i].Clicked += () => OnEmptyPlugClicked(_emptyPlugs[closure]);
            }
        }

        private void OnBrokenPlugClicked(GameObject plug)
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
            plug.SetActive(false);

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

                if (!_emptyPlugList.Any())
                {
                    // todo: complete quest
                    gameObject.SetActive(false);

                    if (_inventory.Contains(InventoryItem.ScrewEdge3))
                        _inventory.Remove(InventoryItem.ScrewEdge3);

                    if (_inventory.Contains(InventoryItem.ScrewRadiation))
                        _inventory.Remove(InventoryItem.ScrewRadiation);
                }
            }

            else if (_inventory.Contains(InventoryItem.ScrewEdge3) ||
                     _inventory.Contains(InventoryItem.ScrewRadiation))
            {
                _textDisplayer.DisplayIncorrectBolt();
            }
        }
    }
}