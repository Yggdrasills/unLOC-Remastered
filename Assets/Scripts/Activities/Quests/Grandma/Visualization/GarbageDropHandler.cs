using DG.Tweening;

using SevenDays.unLOC.Utils.Physics;

using UnityEngine;
using UnityEngine.Events;

namespace SevenDays.unLOC.Activities.Quests.Grandma.Visualization
{
    public class GarbageDropHandler : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _jumpStart;

        [SerializeField]
        private BounceScaler _scaler;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private TrashCans _bindItem;

        [SerializeField]
        private float _jumpPower = 3;

        [SerializeField]
        private float _jumpDuration = 1;

        [SerializeField]
        private int _jumpCount = 1;

        public void Drop(out bool isDropped, Camera targetCamera)
        {
            isDropped = false;
            var raycaster = new Raycaster(_layerMask, targetCamera);

            var trashCan = raycaster.GetHit<TrashCanView>();

            if (trashCan != null && trashCan.BindItem == _bindItem)
            {
                var sequence = DOTween.Sequence();

                _jumpStart?.Invoke();

                sequence.Append(transform.DOJump(trashCan.transform.position, _jumpPower, _jumpCount, _jumpDuration))
                    .Join(transform.DOScale(Vector2.zero, _jumpDuration))
                    .OnComplete(() => Destroy(gameObject));

                isDropped = true;

                return;
            }

            _scaler.BounceScale(() => Destroy(gameObject));
        }
    }
}