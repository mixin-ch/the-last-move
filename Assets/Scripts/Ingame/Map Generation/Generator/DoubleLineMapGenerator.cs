using Mixin.Utils;
using System.Collections.Generic;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class DoubleLineMapGenerator : IMapGenerator
    {
        private SingleLineMapGenerator _generator0;
        private SingleLineMapGenerator _generator1;

        public DoubleLineMapGenerator(float height0, float height1)
        {
            _generator0 = new SingleLineMapGenerator(height0);
            _generator1 = new SingleLineMapGenerator(height1);
        }

        public MapPlan Tick(float blockChunkSize)
        {
            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();

            MapPlan mapPlan0 = _generator0.Tick(blockChunkSize);
            MapPlan mapPlan1 = _generator1.Tick(blockChunkSize);

            blockPlanList.AddRange(mapPlan0.BlockPlanList);
            blockPlanList.AddRange(mapPlan1.BlockPlanList);

            obstaclePlanList.AddRange(mapPlan0.ObstaclePlanList);
            obstaclePlanList.AddRange(mapPlan1.ObstaclePlanList);

            return new MapPlan(blockPlanList, obstaclePlanList);
        }
    }
}
