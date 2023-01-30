using Mixin.TheLastMove.Environment;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockOperator : MonoBehaviour
    {
        [System.Obsolete]
        [SerializeField]
        private SpriteRenderer _sprite;

        [SerializeField]
        private SpriteRenderer _mainSprite;

        [SerializeField]
        private SpriteRenderer _fundament;

        public Vector2 Position => transform.position;

        public SpriteRenderer MainSprite { get => _mainSprite; }

        public SpriteRenderer Fundament { get => _fundament; }

        public void Setup(Vector3 position, float size, BiomeSO biome)
        {
            transform.position = position;
            transform.localScale = Vector2.one * size;

            _mainSprite.sprite = biome.Sprite;
            _mainSprite.gameObject.transform.localPosition =
                biome.Prefab.GetComponent<BlockOperator>().MainSprite.transform.localPosition;

            _fundament.color =
                 biome.Prefab.GetComponent<BlockOperator>()._fundament.color;
        }

        public void Move(Vector2 offset)
        {
            transform.position += (Vector3)offset;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
