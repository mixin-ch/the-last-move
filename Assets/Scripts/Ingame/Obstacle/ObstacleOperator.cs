using System;
using System.Collections;
using UnityEngine;
using Mixin.Utils;

namespace Mixin.TheLastMove.Environment
{
    public class ObstacleOperator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _deathSprite;

        [SerializeField]
        private BoxCollider2D _collider2D;

        [SerializeField]
        private bool _killable;

        [ConditionalField("_killable", true)]
        [SerializeField]
        private float _fadeTime = 1f;

        [ConditionalField("_killable", true)]
        [SerializeField]
        private float _scaleFactor = 1.2f;

        [SerializeField]
        private float _groundOffset = 0;

        public static event Action<ObstacleOperator> OnKilled;

        public bool Killable { get => _killable; }

        public void Setup(ObstacleOperator obstacle, Vector3 position)
        {
            // Attributes
            _killable = obstacle._killable;
            _fadeTime = obstacle._fadeTime;
            _scaleFactor = obstacle._scaleFactor;

            // Reset scale of Sprite
            _spriteRenderer.transform.localScale = new Vector3(1, 1, 1);

            // Set image
            _spriteRenderer.sprite = obstacle._spriteRenderer.sprite;
            _spriteRenderer.color = Color.white;

            // Position and scale
            position.y += obstacle._groundOffset;
            transform.position = position;
            transform.localScale = obstacle.transform.localScale;

            // Collider
            _collider2D.enabled = true;
            _collider2D.offset = obstacle._collider2D.offset;
            _collider2D.size = obstacle._collider2D.size;

            // Tag
            transform.tag = obstacle.transform.tag;
        }

        public void Move(Vector2 offset)
        {
            transform.position += (Vector3)offset;
        }

        public void Kill()
        {
            if (!_killable)
                return;

            StartCoroutine(FadeAndScale());
            EnvironmentManager.Instance.ObstaclesKilled++;
            OnKilled?.Invoke(this);
        }

        private IEnumerator FadeAndScale()
        {
            float elapsedTime = 0f;
            Color originalColor = _spriteRenderer.color;

            _collider2D.enabled = false;
            _spriteRenderer.sprite = _deathSprite;

            while (elapsedTime < _fadeTime)
            {
                elapsedTime += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(originalColor, Color.clear, elapsedTime / _fadeTime);
                _spriteRenderer.transform.localScale *= _scaleFactor;
                yield return null;
            }

            MapManager.Instance.KillObstacle(this);
        }

        public Vector2 Position => transform.position;
    }
}
