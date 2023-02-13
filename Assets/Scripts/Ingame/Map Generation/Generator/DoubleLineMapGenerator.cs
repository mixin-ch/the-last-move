using Mixin.TheLastMove.Environment;
using Mixin.Utils;
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
        private MixinDictionary<ObstacleOperator, float> _obstacleMultiplierDict = new MixinDictionary<ObstacleOperator, float>();
        private MixinDictionary<CollectableDataSet, float> _collectableMultiplierDict = new MixinDictionary<CollectableDataSet, float>();

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
            set { _gapMultiplier = value; _generator0.GapMultiplier = GapMultiplier; _generator1.GapMultiplier = GapMultiplier; }
        }
        public MixinDictionary<ObstacleOperator, float> ObstacleMultiplierDict
        {
            get => _obstacleMultiplierDict;
            set { _obstacleMultiplierDict = value; _generator0.ObstacleMultiplierDict = ObstacleMultiplierDict; _generator1.ObstacleMultiplierDict = ObstacleMultiplierDict; }
        }
        public MixinDictionary<CollectableDataSet, float> CollectableMultiplierDict
        {
            get => _collectableMultiplierDict;
            set { _collectableMultiplierDict = value; _generator0.CollectableMultiplierDict = CollectableMultiplierDict; _generator1.CollectableMultiplierDict = CollectableMultiplierDict; }
        }

        private SingleLineMapGenerator _generator0;
        private SingleLineMapGenerator _generator1;

        public DoubleLineMapGenerator(float height0, float height1, float blockChunkSize = 1, float gapMultiplier = 1
            , MixinDictionary<ObstacleOperator, float> obstacleMultiplierDict = null, MixinDictionary<CollectableDataSet, float> collectableMultiplierDict = null)
        {
            _height0 = height0;
            _height1 = height1;
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            if (obstacleMultiplierDict != null)
                _obstacleMultiplierDict = obstacleMultiplierDict;

            if (collectableMultiplierDict != null)
                _collectableMultiplierDict = collectableMultiplierDict;

            _generator0 = new SingleLineMapGenerator(_height0, _blockChunkSize, _gapMultiplier, _obstacleMultiplierDict);
            _generator1 = new SingleLineMapGenerator(_height1, _blockChunkSize, _gapMultiplier, _obstacleMultiplierDict);
        }

        public MapPlan Tick()
        {
            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();
            List<CollectablePlan> collectablePlanList = new List<CollectablePlan>();

            MapPlan mapPlan0 = _generator0.Tick();
            MapPlan mapPlan1 = _generator1.Tick();

            blockPlanList.AddRange(mapPlan0.BlockPlanList);
            blockPlanList.AddRange(mapPlan1.BlockPlanList);

            obstaclePlanList.AddRange(mapPlan0.ObstaclePlanList);
            obstaclePlanList.AddRange(mapPlan1.ObstaclePlanList);

            collectablePlanList.AddRange(mapPlan0.CollectablePlanList);
            collectablePlanList.AddRange(mapPlan1.CollectablePlanList);

            return new MapPlan(blockPlanList, obstaclePlanList, collectablePlanList);
        }
    }
}
