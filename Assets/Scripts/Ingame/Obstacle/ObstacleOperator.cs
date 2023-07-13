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
        private BoxCollider2D _harmCollider;

        [SerializeField]
        private EdgeCollider2D _groundCollider;

        [SerializeField]
        private bool _killable;

        [SerializeField]
        private bool _upHarm;

        [SerializeField]
        private bool _downHarm;

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

        public bool UpHarm { get => _upHarm; }

        public bool DownHarm { get => _downHarm; }

        public void Setup(ObstacleOperator obstacle, Vector3 position)
        {
            // Attributes
            _fadeTime = obstacle._fadeTime;
            _scaleFactor = obstacle._scaleFactor;

            _killable = obstacle._killable;
            _upHarm = obstacle._upHarm;
            _downHarm = obstacle._downHarm;

            // Reset scale of Sprite
            _spriteRenderer.transform.localScale = new Vector3(1, 1, 1);

            // Set image
            _spriteRenderer.sprite = obstacle._spriteRenderer.sprite;
            _spriteRenderer.color = Color.white;
            _spriteRenderer.sortingLayerID = obstacle._spriteRenderer.sortingLayerID;

            // Position and scale
            position.y += obstacle._groundOffset;
            transform.position = position;
            transform.localScale = obstacle.transform.localScale;

            // Collider
            _harmCollider.enabled = obstacle._harmCollider.enabled;
            _harmCollider.offset = obstacle._harmCollider.offset;
            _harmCollider.size = obstacle._harmCollider.size;

            _groundCollider.enabled = obstacle._groundCollider.enabled;
            _groundCollider.offset = obstacle._groundCollider.offset;
            _groundCollider.points = obstacle._groundCollider.points;

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

            _harmCollider.enabled = false;
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
