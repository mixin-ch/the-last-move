using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class ObstacleOperator : MonoBehaviour
    {
        public static int Counter;

        public static event Action<ObstacleOperator> OnKilled;

        private void Awake()
        {
            ResetCounter();
        }

        public void Setup(Vector3 position)
        {
            transform.position = position;
        }

        public void Move(Vector2 offset)
        {
            transform.position += (Vector3)offset;
        }

        public void Kill()
        {
            MapManager.Instance.KillObstacle(this);
            Counter++;
            OnKilled?.Invoke(this);
        }

        private void ResetCounter()
        {
            Counter = 0;
        }

        public Vector2 Position => transform.position;
    }
}
