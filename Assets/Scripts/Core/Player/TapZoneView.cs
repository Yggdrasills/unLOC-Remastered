using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Core.Player
{
    public class TapZoneView : MonoBehaviour, IPointerClickHandler
    {
        public Action<Vector3> Clicked = delegate { };

        public BoxCollider2D Collider2D { get; private set; }

        private Camera _camera;

        public void SetUp(Camera cam, Vector2 colliderSize, Vector2 position)
        {
            _camera = cam;
            Collider2D = gameObject.AddComponent<BoxCollider2D>();
            Collider2D.size = colliderSize;
            transform.position = position;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var position = _camera.ScreenToWorldPoint(eventData.position);

            Clicked.Invoke(position);
        }
    }
}