using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class MapGenerator : MonoBehaviour
    {
        private const float _insertDistance = 15f;
        private const float _minInsertHeight = -10f;
        private const float _maxInsertHeight = 0f;

        private float _distancePlanned;

        public void Initialize()
        {
            Clear();
        }

        private void Clear()
        {
            _distancePlanned = 0;
        }

        public MapPlan Tick(float offset)
        {
            _distancePlanned += offset;

            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();

            System.Random random = new System.Random();

            while (_distancePlanned > 0)
            {
                if (random.RandomTrue(0.5))
                {
                    float x = _insertDistance - _distancePlanned;
                    float y = (float)random.Range(_minInsertHeight, _maxInsertHeight);
                    blockPlanList.Add(new BlockPlan(new Vector2(x, y)));

                    if (random.RandomTrue(0.25))
                    {
                        obstaclePlanList.Add(new ObstaclePlan(new Vector2(x, y + 1)));
                    }
                }

                _distancePlanned -= EnvironmentManager.BlockSize;
            }

            return new MapPlan(blockPlanList, obstaclePlanList);
        }
    }
}
