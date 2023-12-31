﻿using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenDays.unLOC.Activities.Quests.Grandma.Visualization
{
    public class DraggableItemView : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        public event Action Dropped = delegate { };

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private Transform _rootTransform;

        [SerializeField]
        private GameObject _textDescription;

        private SmoothDisplacerView _displacement;
        private GarbageDropHandler _dropHandler;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            Create(_camera.ScreenToWorldPoint(eventData.position));
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            TranslatePosition(_camera.ScreenToWorldPoint(eventData.position));
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            Drop();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _textDescription.SetActive(true);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _textDescription.SetActive(false);
        }

        private void Create(Vector3 creationPosition)
        {
            var obj = Instantiate(_prefab, creationPosition, Quaternion.identity, _rootTransform);

            _displacement = obj.GetComponent<SmoothDisplacerView>();
            _dropHandler = obj.GetComponent<GarbageDropHandler>();
        }

        private void Drop()
        {
            _dropHandler.Drop(out bool isDropped, _camera);

            if (!isDropped)
                return;

            Dropped?.Invoke();
            gameObject.SetActive(false);
        }

        private void TranslatePosition(Vector2 position)
        {
            _displacement.SetPosition(position);
        }
    }
}