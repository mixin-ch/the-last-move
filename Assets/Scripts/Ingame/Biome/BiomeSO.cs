using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    [CreateAssetMenu(fileName = "BiomeSO", menuName = "Mixin/TheLastMove/Biome")]
    public class BiomeSO : ScriptableObject
    {
        [SerializeField]
        private Sprite _sprite;

        public Sprite Sprite { get => _sprite; }
    }
}
