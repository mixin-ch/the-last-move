using Mixin.TheLastMove.Environment;
using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class MapGenerator : IMapGenerator
    {
        private float _blockChunkSize;
        private float _gapMultiplier;
        private MixinDictionary<ObstacleOperator, float> _obstacleMultiplierDict = new MixinDictionary<ObstacleOperator, float>();

        public float BlockChunkSize { get => _blockChunkSize; set => _blockChunkSize = value; }
        public float GapMultiplier { get => _gapMultiplier; set => _gapMultiplier = value; }
        public MixinDictionary<ObstacleOperator, float> ObstacleMultiplierDict { get => _obstacleMultiplierDict; set => _obstacleMultiplierDict = value; }

        private IMapGenerator _currentGenerator;
        private float _lastHeight;
        private int _steps;
        private bool _firstStep;

        public MapGenerator(float blockChunkSize = 1, float gapMultiplier = 1, MixinDictionary<ObstacleOperator, float> obstacleMultiplierDict = null)
        {
            _blockChunkSize = blockChunkSize;
            _gapMultiplier = gapMultiplier;

            if (obstacleMultiplierDict != null)
                _obstacleMultiplierDict = obstacleMultiplierDict;

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
                        SetupSingleLine(_blockChunkSize, _gapMultiplier, _obstacleMultiplierDict);
                        break;
                    case 1:
                        SetupDoubleLine(_blockChunkSize, _gapMultiplier, _obstacleMultiplierDict);
                        break;
                }
            }

            _steps--;

            return _currentGenerator.Tick();
        }

        private void SetupSingleLineFirst(float blockChunkSize)
        {
            float height = MapManager.StartBlockHeightFraction;
            _currentGenerator = new SingleLineMapGenerator(height, blockChunkSize, 0, new MixinDictionary<ObstacleOperator, float>());

            _steps = (30 * blockChunkSize).RoundToInt().LowerBound(1);

            _lastHeight = height;
        }

        private void SetupSingleLine(float blockChunkSize, float gapMultiplier, MixinDictionary<ObstacleOperator, float> obstacleMultiplierDict)
        {
            System.Random random = new System.Random();

            float height = (float)random.NextDouble().Between(_lastHeight - 0.5, _lastHeight + 0.5);
            _currentGenerator = new SingleLineMapGenerator(height, blockChunkSize, gapMultiplier, obstacleMultiplierDict);

            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height;
        }

        private void SetupDoubleLine(float blockChunkSize, float gapMultiplier, MixinDictionary<ObstacleOperator, float> obstacleMultiplierDict)
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

            _currentGenerator = new DoubleLineMapGenerator(height0, height1, blockChunkSize, gapMultiplier, obstacleMultiplierDict);
            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height0;
        }
    }
}
