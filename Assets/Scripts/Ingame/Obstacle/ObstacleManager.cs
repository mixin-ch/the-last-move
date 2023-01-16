using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class ObstacleManager : Singleton<ObstacleManager>
    {
        [SerializeField]
        private GameObject _blockContainer;
        [SerializeField]
        private GameObject _obstaclePrefab;
    }
}
