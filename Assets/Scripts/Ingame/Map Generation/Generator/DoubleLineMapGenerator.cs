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
        private float _obstacleProbability;
        private float _collectableProbability;
        private MixinDictionary<ObstacleOperator, float> _obstacleWeightDict = new MixinDictionary<ObstacleOperator, float>();
        private MixinDictionary<CollectableDataSet, float> _collectableWeightDict = new MixinDictionary<CollectableDataSet, float>();

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
        public float ObstacleProbability
        {
            get => _obstacleProbability;
            set { _obstacleProbability = value; _generator0.ObstacleProbability = ObstacleProbability; _generator1.ObstacleProbability = ObstacleProbability; }
        }
        public float CollectableProbability
        {
            get => _collectableProbability;
            set { _collectableProbability = value; _generator0.CollectableProbability = CollectableProbability; _generator1.CollectableProbability = CollectableProbability; }
        }
        public MixinDictionary<ObstacleOperator, float> ObstacleWeightDict
        {
            get => _obstacleWeightDict;
            set { _obstacleWeightDict = value; _generator0.ObstacleWeightDict = ObstacleWeightDict; _generator1.ObstacleWeightDict = ObstacleWeightDict; }
        }
        public MixinDictionary<CollectableDataSet, float> CollectableWeightDict
        {
            get => _collectableWeightDict;
            set { _collectableWeightDict = value; _generator0.CollectableWeightDict = CollectableWeightDict; _generator1.CollectableWeightDict = CollectableWeightDict; }
        }

        private SingleLineMapGenerator _generator0;
        private SingleLineMapGenerator _generator1;

        public DoubleLineMapGenerator(float height0, float height1, float blockChunkSize = 1, float gapMultiplier = 1
            , float obstacleProbability = 0, float collectableProbability = 0
            , MixinDictionary<ObstacleOperator, float> obstacleWeightDict = null, MixinDictionary<CollectableDataSet, float> collectableWeightDict = null)
        {
            _height0 = height0;
            _height1 = height1;
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            _obstacleProbability = obstacleProbability;
            _collectableProbability = collectableProbability;

            if (obstacleWeightDict != null)
                _obstacleWeightDict = obstacleWeightDict;

            if (collectableWeightDict != null)
                _collectableWeightDict = collectableWeightDict;

            _generator0 = new SingleLineMapGenerator(_height0, _blockChunkSize, _gapMultiplier
                , _obstacleProbability, _collectableProbability, _obstacleWeightDict, _collectableWeightDict);
            _generator1 = new SingleLineMapGenerator(_height1, _blockChunkSize, _gapMultiplier
                 , _obstacleProbability, _collectableProbability, _obstacleWeightDict, _collectableWeightDict);
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
