using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Inventory.Views
{
    public class InventoryCellView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _counterText;

        public int Amount { get; private set; }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetClickAction(Action action)
        {
            _button.onClick.AddListener(() => action?.Invoke());
        }

        public void IncrementAmount()
        {
            SetCounterText(true);
        }

        public void DecrementAmount()
        {
            SetCounterText(false);
        }

        private void SetCounterText(bool increase)
        {
            Amount = increase ? Amount + 1 : Amount - 1;

            _counterText.text = Amount.ToString();
        }
    }
}