using UnityEngine;

namespace Mixin.TheLastMove
{
    public class ObstacleOperator : MonoBehaviour
    {
        public void Setup(Vector2 position, float size)
        {
            transform.position = position;
            transform.localScale = Vector2.one * size;
        }

        public void Move(Vector2 offset)
        {
            transform.position = transform.position + (Vector3)offset;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;
    }
}
