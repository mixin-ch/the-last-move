using Mixin.TheLastMove.Environment.Collectable;
using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class CollectablePlan
    {
        private CollectableOperator _collectable;
        [Range(0, 1)]
        private float _height;
        private Vector2 _offset;

        public CollectableOperator Collectable { get => _collectable; }
        public float Height { get => _height; }
        public Vector2 Offset { get => _offset; }

        public CollectablePlan(CollectableOperator collectable, float height, Vector2 offset)
        {
            _collectable = collectable;
            _height = height;
            _offset = offset;
        }
    }
}
