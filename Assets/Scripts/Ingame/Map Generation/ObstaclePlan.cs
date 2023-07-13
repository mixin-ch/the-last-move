using Mixin.TheLastMove.Environment;
using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class ObstaclePlan
    {
        private ObstacleOperator _obstacle;
        [Range(0, 1)]
        private float _height;

        public float Height { get => _height; }
        public ObstacleOperator Obstacle { get => _obstacle; }

        public ObstaclePlan(ObstacleOperator obstacle, float position)
        {
            _obstacle = obstacle;
            _height = position;
        }
    }
}
