using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockMaker : MonoBehaviour
    {
        private List<BlockRowMaker> _blockRowMakerList = new List<BlockRowMaker>();

        public void Initialize()
        {
            _blockRowMakerList.Clear();

            for (int i = 0; i < EnvironmentManager.BlockRows; i++)
            {
                BlockRowMaker blockRowMaker = new BlockRowMaker();
                blockRowMaker.Initialize();
                _blockRowMakerList.Add(blockRowMaker);
            }
        }

        public List<bool> PickNext()
        {
            List<bool> pickList = new List<bool>();

            foreach (BlockRowMaker blockRowMaker in _blockRowMakerList)
                pickList.Add(blockRowMaker.PickNext());

            return pickList;
        }
    }
}
