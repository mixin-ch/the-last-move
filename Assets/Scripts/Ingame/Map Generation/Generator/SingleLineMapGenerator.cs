using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.Utils;
using System.Collections.Generic;

namespace Mixin.TheLastMove
{
    [System.Serializable]
    public class SingleLineMapGenerator : IMapGenerator
    {
        private float _height;
        private float _blockChunkSize;
        private float _gapMultiplier;
        private float _obstacleProbability;
        private float _collectableProbability;
        private MixinDictionary<ObstacleOperator, float> _obstacleWeightDict = new MixinDictionary<ObstacleOperator, float>();
        private MixinDictionary<CollectableDataSet, float> _collectableWeightDict = new MixinDictionary<CollectableDataSet, float>();

        public float Height { get => _height; set => _height = value; }
        public float BlockChunkSize { get => _blockChunkSize; set => _blockChunkSize = value; }
        public float GapMultiplier { get => _gapMultiplier; set => _gapMultiplier = value; }
        public float ObstacleProbability { get => _obstacleProbability; set => _obstacleProbability = value; }
        public float CollectableProbability { get => _collectableProbability; set => _collectableProbability = value; }
        public MixinDictionary<ObstacleOperator, float> ObstacleWeightDict { get => _obstacleWeightDict; set => _obstacleWeightDict = value; }
        public MixinDictionary<CollectableDataSet, float> CollectableWeightDict { get => _collectableWeightDict; set => _collectableWeightDict = value; }

        private bool _filled;
        private int _steps;

        public SingleLineMapGenerator(float height, float blockChunkSize = 1, float gapMultiplier = 1
            , float obstacleProbability = 0, float collectableProbability = 0
            , MixinDictionary<ObstacleOperator, float> obstacleWeightDict = null, MixinDictionary<CollectableDataSet, float> collectableWeightDict = null)
        {
            _height = height;
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            _obstacleProbability = obstacleProbability;
            _collectableProbability = collectableProbability;

            if (obstacleWeightDict != null)
                _obstacleWeightDict = obstacleWeightDict;

            if (collectableWeightDict != null)
                _collectableWeightDict = collectableWeightDict;
        }

        public MapPlan Tick()
        {
            System.Random random = new System.Random();

            if (_steps <= 0)
            {
                _filled = !_filled;

                if (GapMultiplier <= 0)
                    _filled = true;

                int min = 1;
                int max = 1;

                if (_filled)
                {
                    min = _blockChunkSize.RoundToInt().LowerBound(min);
                    max = (5 * _blockChunkSize).RoundToInt().LowerBound(max);
                }
                else
                {
                    min = _blockChunkSize.RoundToInt().LowerBound(min);
                    max = (3 * _gapMultiplier * _blockChunkSize).RoundToInt().LowerBound(max);
                }

                _steps = random.Range(min, max);
            }

            List<BlockPlan> blockPlanList = new List<BlockPlan>();
            List<ObstaclePlan> obstaclePlanList = new List<ObstaclePlan>();
            List<CollectablePlan> collectablePlanList = new List<CollectablePlan>();

            if (_filled)
            {
                blockPlanList.Add(new BlockPlan(_height));

                if (!_obstacleWeightDict.IsEmpty())
                {
                    ObstacleOperator obstacle = _obstacleWeightDict.PickWeightedRandom(random);

                    if (random.RandomTrue(_obstacleProbability))
                        obstaclePlanList.Add(new ObstaclePlan(obstacle, _height));
                }

                if (!_collectableWeightDict.IsEmpty())
                {
                    CollectableDataSet collectableDataSet = _collectableWeightDict.PickWeightedRandom(random);

                    if (random.RandomTrue(_collectableProbability))
                        foreach (CollectableOperator collectable in collectableDataSet.Collectables)
                            collectablePlanList.Add(new CollectablePlan(collectable, _height, collectable.Position));
                }
            }

            _steps--;

            return new MapPlan(blockPlanList, obstaclePlanList, collectablePlanList);
        }
    }
}
