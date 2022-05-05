using System;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Services;

using UnityEngine;

namespace SevenDays.unLOC.Views
{
    public abstract class PickableBase : InteractableItem
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