using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment.Collectable
{
    public class CollectableOperator : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField]
        private GameObject _collectableModel;

        [SerializeField]
        private SpriteFadeAndScaler _spriteFadeAndScaler;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _collectedSprite;

        /* Data */
        [SerializeField]
        private Sprite _sprite;

        public Sprite Sprite { get => _sprite; set => _sprite = value; }
        public Vector2 Position { get => transform.localPosition; set => transform.localPosition = value; }
        public Vector2 Scale { get => transform.localScale; set => transform.localScale = value; }

        public static event Action<CollectableOperator> OnCollected;

        public void Setup(CollectableOperator @operator, Vector3 position)
        {
            _spriteRenderer.sprite = @operator.Sprite;
            _spriteRenderer.transform.localScale = @operator._spriteRenderer.transform.localScale;

            transform.position = position;

            // Collider
            _collider.enabled = true;
            _collider.offset = @operator._collider.offset;
            _collider.size = @operator._collider.size;
        }

        public void Collect()
        {
            // Handle the logic for what happens when the player collects the collectable
            // e.g. play a sound, give the player points, etc.
            // or you can call an event that you can listen in the player script.
            Debug.Log("Collectable Collected!");

            _spriteRenderer.sprite = _collectedSprite;
            StartCoroutine(_spriteFadeAndScaler.FadeAndScale());

            EnvironmentManager.Instance.CollectablesCollected++;

            OnCollected?.Invoke(this);
        }
    }
}