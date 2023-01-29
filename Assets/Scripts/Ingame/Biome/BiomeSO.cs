using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    [CreateAssetMenu(fileName = "BiomeSO", menuName = "Mixin/TheLastMove/Biome")]
    public class BiomeSO : ScriptableObject
    {
        [SerializeField]
        private Sprite _background;

        [System.Obsolete]
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private GameObject _prefab;

        public Sprite Sprite { get => _sprite; }
        public Sprite Background { get => _background; }
        public GameObject Prefab { get => _prefab; }
    }
}
