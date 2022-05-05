using System.Collections.Generic;
using System.Linq;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Screwdriver;
using SevenDays.unLOC.Services;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    public class FlowerQuest : MonoBehaviour
    {
        [SerializeField]
        private FlowerQuestTextDisplayer _textDisplayer;

        [SerializeField]
        private ScrewdriverView _screwdriver;

        [SerializeField]
        private ClickableItem[] _brokenPlugs;

        [SerializeField]
        private GameObject _firstStage;

        [SerializeField]
        private GameObject _secondStage;

        [SerializeField]
        private Nozzle _targetNozzle;

        private IInventoryService _inventory;

        private List<GameObject> _brokenPlugList;

        [Inject]
        private void Construct(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        private void Awake()
        {
            _brokenPlugList = new List<GameObject>(_brokenPlugs.Length);

            for (int i = 0; i < _brokenPlugs.Length; i++)
            {
                int closure = i;

                _brokenPlugList.Add(_brokenPlugs[i].gameObject);

                _brokenPlugs[i].Clicked += () => OnBrokenPlugClicked(_brokenPlugs[closure].gameObject);
            }

            _firstStage.SetActive(true);
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
            }
        }
    }
}