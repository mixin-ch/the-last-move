using Mixin.Utils;

namespace Mixin.TheLastMove
{
    public class MapGenerator : IMapGenerator
    {
        private IMapGenerator _currentGenerator;
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
                _currentGenerator = new SingleLineMapGenerator((float)random.NextDouble());
                int min = (10 * blockChunkSize).RoundToInt().LowerBound(1);
                int max = (50 * blockChunkSize).RoundToInt().LowerBound(1);
                _steps = random.Range(min, max);
            }

            _steps--;

            return _currentGenerator.Tick(blockChunkSize);
        }
    }
}
