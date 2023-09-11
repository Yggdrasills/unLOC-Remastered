using System;

using UnityEngine;

namespace SevenDays.unLOC.Core.Player
{
    [Serializable]
    public class CharacterScale
    {
        [SerializeField]
        private Transform _body;

        [SerializeField]
        private BoxCollider2D _collider;

        public void SetScale(Vector2 bodySize, Vector2 colliderSize)
        {
            _body.localScale = bodySize;
            _collider.size = colliderSize;
        }
    }
}