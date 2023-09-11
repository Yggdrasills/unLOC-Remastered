using SevenDays.unLOC.Activities.Items;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Flower
{
    [RequireComponent(typeof(Collider2D))]
    public class EmptyPlugView : ClickableItem
    {
        [SerializeField]
        private Collider2D _collider;

        [SerializeField]
        private GameObject _fixedBolt;

        private void OnValidate()
        {
            if (_collider == null)
                _collider = GetComponent<Collider2D>();

            if (_fixedBolt == null)
                _fixedBolt = transform.GetChild(0).gameObject;
        }

        public void ActivateBolt()
        {
            _collider.enabled = false;
            _fixedBolt.SetActive(true);
        }
    }
}