using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class ObstacleOperator : MonoBehaviour
    {
        public void Setup(Vector2 position)
        {
            transform.position = position;
        }

        public void Move(Vector2 offset)
        {
            transform.position = transform.position + (Vector3)offset;
        }

        public void Kill()
        {
            MapManager.Instance.KillObstacle(this);
        }

        public Vector2 Position => transform.position;
    }
}
