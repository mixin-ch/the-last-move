using UnityEngine;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class BlockPlan
    {
        [Range(0,1)]
        private float _height;

        public float Height { get => _height; }

        public BlockPlan(float position)
        {
            _height = position;
        }
    }
}
