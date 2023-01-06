using Mixin.Utils;
using System.Collections.Generic;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class SingleLineMapGenerator : IMapGenerator
    {
        private float _height;

        private bool _filled;
        private int _steps;

        public SingleLineMapGenerator(float height)
        {
            _height = height;
        }

        public MapPlan Tick(float blockChunkSize)
        {
            System.Random random = new System.Random();

            if (_steps <= 0)
            {
                _filled = !_filled;
                int min = 1;
                int max = 1;

                if (_filled)
                {
                    min = blockChunkSize.RoundToInt().LowerBound(min);
                    max = (5 * blockChunkSize).RoundToInt().LowerBound(max);
                }
                else
                {
                    min = blockChunkSize.RoundToInt().LowerBound(min);
                    max = (3 * blockChunkSize).RoundToInt().LowerBound(max);
                }

                _steps = random.Range(min, max);
            }

            _steps--;

            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();

            if (_filled)
                blockPlanList.Add(new BlockPlan(_height));

            return new MapPlan(blockPlanList, obstaclePlanList);
        }
    }
}
