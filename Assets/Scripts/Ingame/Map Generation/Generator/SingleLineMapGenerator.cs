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
        private MixinDictionary<ObstacleOperator, float> _obstacleMultiplierDict = new MixinDictionary<ObstacleOperator, float>();
        private MixinDictionary<CollectableDataSet, float> _collectableMultiplierDict = new MixinDictionary<CollectableDataSet, float>();

        public float Height { get => _height; set => _height = value; }
        public float BlockChunkSize { get => _blockChunkSize; set => _blockChunkSize = value; }
        public float GapMultiplier { get => _gapMultiplier; set => _gapMultiplier = value; }
        public MixinDictionary<ObstacleOperator, float> ObstacleMultiplierDict { get => _obstacleMultiplierDict; set => _obstacleMultiplierDict = value; }
        public MixinDictionary<CollectableDataSet, float> CollectableMultiplierDict { get => _collectableMultiplierDict; set => _collectableMultiplierDict = value; }

        private bool _filled;
        private int _steps;

        public SingleLineMapGenerator(float height, float blockChunkSize = 1, float gapMultiplier = 1, MixinDictionary<ObstacleOperator, float> obstacleMultiplierDict = null, MixinDictionary<CollectableDataSet, float> collectableMultiplierDict = null)
        {
            _height = height;
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            if (obstacleMultiplierDict != null)
                _obstacleMultiplierDict = obstacleMultiplierDict;

            if (collectableMultiplierDict != null)
                _collectableMultiplierDict = collectableMultiplierDict;
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

                if (!_obstacleMultiplierDict.IsEmpty())
                {
                    float sum = 0;

                    foreach (float probability in _obstacleMultiplierDict.Values)
                        sum += probability;

                    ObstacleOperator obstacle = _obstacleMultiplierDict.PickWeightedRandom(random);

                    if (random.RandomTrue(sum))
                        obstaclePlanList.Add(new ObstaclePlan(obstacle, _height));
                }

                if (!_collectableMultiplierDict.IsEmpty())
                {
                    float sum = 0;

                    foreach (float probability in _collectableMultiplierDict.Values)
                        sum += probability;

                    CollectableDataSet collectableDataSet = _collectableMultiplierDict.PickWeightedRandom(random);

                    if (random.RandomTrue(sum))
                        foreach (CollectableOperator collectable in collectableDataSet.Collectables)
                            collectablePlanList.Add(new CollectablePlan(collectable, _height, collectable.Position));
                }
            }

            _steps--;

            return new MapPlan(blockPlanList, obstaclePlanList, collectablePlanList);
        }
    }
}
