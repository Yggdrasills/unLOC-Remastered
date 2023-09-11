using SevenDays.unLOC.Inventory.Services;

using TMPro;

using UnityEngine;
using UnityEngine.Assertions;
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

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetClickAction(ItemClickStrategy itemClickStrategy)
        {
            _button.onClick.AddListener(() => itemClickStrategy?.ClickStrategy?.Invoke());
        }

        public void SetCounterText(int amount)
        {
            Assert.IsTrue(amount > 0,
                $"[{nameof(InventoryCellView)}] Items amount should be more than zero");

            _counterText.text = amount == 1 ? string.Empty : amount.ToString();
        }
    }
}