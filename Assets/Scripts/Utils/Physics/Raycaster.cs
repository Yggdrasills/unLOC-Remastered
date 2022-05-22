using UnityEngine;

namespace SevenDays.unLOC.Utils.Physics
{
    public class Raycaster
    {
        private const float RayDistance = 10f;

        private readonly LayerMask _layerMask;

        private readonly Camera _camera;

        public Raycaster(LayerMask layerMask, Camera camera)
        {
            _layerMask = layerMask;
            _camera = camera;
        }

        /// <summary>
        /// Returns Component by type T. Returns default if component not found on hitted object
        /// </summary>
        /// <returns></returns>
        public T GetHit<T>()
        {
            var screenTouchPosition = Input.mousePosition;

            var touchPosition = _camera.ScreenToWorldPoint(screenTouchPosition);
            var hit = Physics2D.Raycast(touchPosition, Vector2.zero, RayDistance, _layerMask);

            if (hit.collider != null)
                return hit.collider.GetComponent<T>();

            return default;
        }
    }
}