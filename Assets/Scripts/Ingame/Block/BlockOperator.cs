using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockOperator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sprite;

        public void Setup(Vector2 position, float size, Sprite sprite)
        {
            transform.position = position;
            transform.localScale = Vector2.one * size;
            _sprite.sprite = sprite;
        }

        public void Move(Vector2 offset)
        {
            transform.position = transform.position + (Vector3)offset;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;
    }
}
