using System.Collections.Generic;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class DoubleLineMapGenerator : IMapGenerator
    {
        private float _height0;
        private float _height1;
        private float _blockChunkSize;
        private float _gapMultiplier;
        private float _obstacleMultiplier;

        public float Height0
        {
            get => _height0;
            set { _height0 = value; _generator0.Height = value; }
        }
        public float Height1
        {
            get => _height1;
            set { _height1 = value; _generator1.Height = value; }
        }
        public float BlockChunkSize
        {
            get => _blockChunkSize;
            set { _blockChunkSize = value; _generator0.BlockChunkSize = value; _generator1.BlockChunkSize = value; }
        }
        public float GapMultiplier
        {
            get => _gapMultiplier;
            set { _gapMultiplier = value; _generator0.GapMultiplier = SingleLineGapMultiplier; _generator1.GapMultiplier = SingleLineGapMultiplier; }
        }
        public float ObstacleMultiplier
        {
            get => _obstacleMultiplier;
            set { _obstacleMultiplier = value; _generator0.ObstacleMultiplier = SingleLineObstacleMultiplier; _generator1.ObstacleMultiplier = SingleLineObstacleMultiplier; }
        }

        private float SingleLineGapMultiplier => GapMultiplier * 2;
        private float SingleLineObstacleMultiplier => ObstacleMultiplier * 2;

        private SingleLineMapGenerator _generator0;
        private SingleLineMapGenerator _generator1;

        public DoubleLineMapGenerator(float height0, float height1, float blockChunkSize = 1, float gapMultiplier = 1, float obstacleMultiplier = 1)
        {
            _height0 = height0;
            _height1 = height1;
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;
            _obstacleMultiplier = obstacleMultiplier;

            _generator0 = new SingleLineMapGenerator(height0, blockChunkSize, SingleLineGapMultiplier, SingleLineObstacleMultiplier);
            _generator1 = new SingleLineMapGenerator(height1, blockChunkSize, SingleLineGapMultiplier, SingleLineObstacleMultiplier);
        }

        public MapPlan Tick()
        {
            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();

            MapPlan mapPlan0 = _generator0.Tick();
            MapPlan mapPlan1 = _generator1.Tick();

            blockPlanList.AddRange(mapPlan0.BlockPlanList);
            blockPlanList.AddRange(mapPlan1.BlockPlanList);

            obstaclePlanList.AddRange(mapPlan0.ObstaclePlanList);
            obstaclePlanList.AddRange(mapPlan1.ObstaclePlanList);

            return new MapPlan(blockPlanList, obstaclePlanList);
        }
    }
}
