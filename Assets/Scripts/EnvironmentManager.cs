using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private GameObject _environmentContainer;
        [SerializeField]
        private GameObject _blockPrefab;
    }
}
