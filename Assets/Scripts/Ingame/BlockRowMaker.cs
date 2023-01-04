using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockRowMaker : MonoBehaviour
    {
        private bool _placeBlock;
        private int _placements;

        public void Initialize()
        {
            _placeBlock = true;
            _placements = (int)(30 / EnvironmentManager.BlockSize).Ceiling();
        }

        public bool PickNext()
        {
            if (_placements <= 0)
            {
                _placeBlock = !_placeBlock;
                System.Random random = new System.Random();
                float multiplier = Mathf.Sqrt(EnvironmentManager.Instance.Hectic) / EnvironmentManager.BlockSize;

                if (_placeBlock)
                    _placements = random.Range(1, (int)(multiplier * 5).Floor().LowerBound(1));
                else
                    _placements = random.Range(1, (int)(multiplier * 3 * EnvironmentManager.BlockRows).Floor().LowerBound(1));
            }

            _placements--;
            return _placeBlock;
        }
    }
}
