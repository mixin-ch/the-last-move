using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class BlockPlan
    {
        private Vector2 _position;

        public Vector2 Position { get => _position; }

        public BlockPlan(Vector2 position)
        {
            _position = position;
        }
    }
}
