using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockMaker : MonoBehaviour
    {
        private bool _placeBlock;
        private int _placements;

        public void ResetValues()
        {
            _placeBlock = true;
            _placements = 10;
        }

        public bool PickNext()
        {
            if (_placements <= 0)
            {
                _placeBlock = !_placeBlock;
                System.Random random = new System.Random();

                if (_placeBlock)
                    _placements = random.Range(1, 5);
                else
                    _placements = random.Range(1, 3);
            }

            _placements--;
            return _placeBlock;
        }
    }
}
