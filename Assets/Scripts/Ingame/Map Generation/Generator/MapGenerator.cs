using Mixin.TheLastMove.Environment;
using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class MapGenerator : IMapGenerator
    {
        private float _blockChunkSize;
        private float _gapMultiplier;
        private float _obstacleProbability;
        private float _collectableProbability;
        private MixinDictionary<ObstacleOperator, float> _obstacleWeightDict = new MixinDictionary<ObstacleOperator, float>();
        private MixinDictionary<CollectableDataSet, float> _collectableWeightDict = new MixinDictionary<CollectableDataSet, float>();

        public float BlockChunkSize { get => _blockChunkSize; set => _blockChunkSize = value; }
        public float GapMultiplier { get => _gapMultiplier; set => _gapMultiplier = value; }
        public float ObstacleProbability { get => _obstacleProbability; set => _obstacleProbability = value; }
        public float CollectableProbability { get => _collectableProbability; set => _collectableProbability = value; }
        public MixinDictionary<ObstacleOperator, float> ObstacleWeightDict { get => _obstacleWeightDict; set => _obstacleWeightDict = value; }
        public MixinDictionary<CollectableDataSet, float> CollectableWeightDict { get => _collectableWeightDict; set => _collectableWeightDict = value; }


        private IMapGenerator _currentGenerator;
        private float _lastHeight;
        private int _steps;
        private bool _firstStep;

        public MapGenerator(float blockChunkSize = 1, float gapMultiplier = 1
            , float obstacleProbability = 0, float collectableProbability = 0
            , MixinDictionary<ObstacleOperator, float> obstacleWeightDict = null, MixinDictionary<CollectableDataSet, float> collectableWeightDict = null)
        {
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            _obstacleProbability = obstacleProbability;
            _collectableProbability = collectableProbability;

            if (obstacleWeightDict != null)
                _obstacleWeightDict = obstacleWeightDict;

            if (collectableWeightDict != null)
                _collectableWeightDict = collectableWeightDict;

            _firstStep = true;
        }

        public MapPlan Tick()
        {
            if (_firstStep)
            {
                _firstStep = false;
                SetupSingleLineFirst(_blockChunkSize);
            }

            System.Random random = new System.Random();

            if (_steps <= 0)
            {
                switch (random.Range(0, 1))
                {
                    case 0:
                        SetupSingleLine(_blockChunkSize, _gapMultiplier, _obstacleProbability, _collectableProbability, _obstacleWeightDict, _collectableWeightDict);
                        break;
                    case 1:
                        SetupDoubleLine(_blockChunkSize, _gapMultiplier, _obstacleProbability, _collectableProbability, _obstacleWeightDict, _collectableWeightDict);
                        break;
                }
            }

            _steps--;

            return _currentGenerator.Tick();
        }

        private void SetupSingleLineFirst(float blockChunkSize)
        {
            float height = MapManager.StartBlockHeightFraction;
            _currentGenerator = new SingleLineMapGenerator(height, blockChunkSize, 0);

            _steps = (30 * blockChunkSize).RoundToInt().LowerBound(1);

            _lastHeight = height;
        }

        private void SetupSingleLine(float blockChunkSize, float gapMultiplier
            , float obstacleProbability, float collectableProbability
            , MixinDictionary<ObstacleOperator, float> obstacleWeightDict, MixinDictionary<CollectableDataSet, float> collectableWeightDict)
        {
            System.Random random = new System.Random();

            float height = (float)random.NextDouble().Between(_lastHeight - 0.5, _lastHeight + 0.5);
            _currentGenerator = new SingleLineMapGenerator(height, blockChunkSize, gapMultiplier
                , obstacleProbability, collectableProbability, obstacleWeightDict, collectableWeightDict);

            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height;
        }

        private void SetupDoubleLine(float blockChunkSize, float gapMultiplier
            , float obstacleProbability, float collectableProbability
            , MixinDictionary<ObstacleOperator, float> obstacleWeightDict, MixinDictionary<CollectableDataSet, float> collectableWeightDict)
        {
            System.Random random = new System.Random();

            float height0;
            float height1;

            if (random.RandomTrue(0.5))
                height0 = _lastHeight;
            else
                height0 = (float)random.NextDouble().Between(_lastHeight - 0.5, _lastHeight + 0.5);

            height1 = (float)random.NextDouble();

            while (Mathf.Abs(height1 - height0) < 0.35)
                height1 = (float)random.NextDouble();

            _currentGenerator = new DoubleLineMapGenerator(height0, height1, blockChunkSize, gapMultiplier
                , obstacleProbability, collectableProbability, obstacleWeightDict, collectableWeightDict);
            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height0;
        }
    }
}
