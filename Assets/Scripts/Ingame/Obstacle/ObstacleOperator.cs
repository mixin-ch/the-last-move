using System;
using System.Collections;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class ObstacleOperator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private Sprite _deathSprite;

        [SerializeField]
        private Collider2D _collider2D;

        [SerializeField]
        private float _fadeTime = 1f;

        [SerializeField]
        private float _scaleFactor = 1.2f;

        public static int Counter;

        public static event Action<ObstacleOperator> OnKilled;

        private void Awake()
        {
            ResetCounter();
        }

        public void Setup(Vector3 position)
        {
            _spriteRenderer.sprite = _sprite;
            _spriteRenderer.color = Color.white;
            transform.position = position;
            _spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            _collider2D.enabled = true;
        }

        public void Move(Vector2 offset)
        {
            transform.position += (Vector3)offset;
        }

        public void Kill()
        {
            StartCoroutine(FadeAndScale());
            Counter++;
            OnKilled?.Invoke(this);
        }

        private void ResetCounter()
        {
            Counter = 0;
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
