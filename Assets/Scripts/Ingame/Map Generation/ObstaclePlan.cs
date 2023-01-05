using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class ObstaclePlan
    {
        private Vector2 _position;

        public Vector2 Position { get => _position; }

        public ObstaclePlan(Vector2 position)
        {
            _position = position;
        }
    }
}
