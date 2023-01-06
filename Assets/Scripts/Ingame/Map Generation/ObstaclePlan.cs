using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class ObstaclePlan
    {
        [Range(0, 1)]
        private float _height;

        public float Height { get => _height; }

        public ObstaclePlan(float position)
        {
            _height = position;
        }
    }
}
