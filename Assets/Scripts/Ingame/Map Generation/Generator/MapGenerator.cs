using Mixin.Utils;

namespace Mixin.TheLastMove
{
    public class MapGenerator : IMapGenerator
    {
        private IMapGenerator _currentGenerator;
        private float _lastHeight;
        private int _steps;

        public MapGenerator()
        {
            _steps = 0;
        }

        public MapPlan Tick(float blockChunkSize)
        {
            System.Random random = new System.Random();

            if (_steps <= 0)
            {
                switch (random.Range(0, 1))
                {
                    case 0:
                        SetupSingleLine(blockChunkSize);
                        break;
                    case 1:
                        SetupDoubleLine(blockChunkSize);
                        break;
                }
            }

            _steps--;

            return _currentGenerator.Tick(blockChunkSize);
        }

        private void SetupSingleLine(float blockChunkSize)
        {
            System.Random random = new System.Random();

            float height = (float)random.NextDouble().Between(_lastHeight - 0.5, _lastHeight + 0.5);
            _currentGenerator = new SingleLineMapGenerator(_lastHeight);

            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height;
        }

        private void SetupDoubleLine(float blockChunkSize)
        {
            System.Random random = new System.Random();

            float height0;
            float height1;

            if (random.RandomTrue(0.5))
                height0 = _lastHeight;
            else
                height0 = (float)random.NextDouble().Between(_lastHeight - 0.5, _lastHeight + 0.5);

            height1 = (float)random.NextDouble();

            _currentGenerator = new DoubleLineMapGenerator(height0, height1);
            int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
            int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
            _steps = random.Range(min, max);

            _lastHeight = height0;
        }
    }
}
