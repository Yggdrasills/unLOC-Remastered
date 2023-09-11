using TMPro;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    // todo: add localization
    public class FlowerQuestTextDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private void OnEnable()
        {
            _text.text = string.Empty;
        }

        public void DisplayScrewdriverNotFound()
        {
            _text.text = "Тут нужна отвертка";
        }

        public void DisplayIncorrectNozzle()
        {
            _text.text = "Насадка не подходит";
        }

        public void DisplayScrewsDescription()
        {
            _text.text = "Так, вроде такие винты были в шкафчике";
        }

        public void DisplayNeedNewScrew()
        {
            _text.text = "Нужен новый болтик";
        }

        public void DisplayIncorrectBolt()
        {
            _text.text = "Неправильный болт";
        }
    }
}