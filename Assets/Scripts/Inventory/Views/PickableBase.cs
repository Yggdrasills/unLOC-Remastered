using System;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Inventory.Views
{
    public abstract class PickableBase : MonoBehaviour
    {
        [SerializeField]
        private Sprite _icon;

        public virtual InventoryItem Type => InventoryItem.None;

        public Sprite Icon => _icon;

        public virtual UniTask VisualisePickAsync()
        {
            gameObject.SetActive(false);

            return UniTask.CompletedTask;
        }

        public virtual Action GetInventoryClickStrategy()
        {
            return delegate { };
        }
    }
}