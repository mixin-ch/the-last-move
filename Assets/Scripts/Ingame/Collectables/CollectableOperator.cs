using Mixin.Utils;
using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment.Collectable
{
    public class CollectableOperator : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _collider;

        [SerializeField]
        private SpriteFadeAndScaler _spriteFadeAndScaler;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _collectedSprite;

        [SerializeField]
        private PulseAnimator _pulseAnimator;

        [SerializeField]
        private MinMaxFloat _height;

        [SerializeField]
        private bool _pulse;

        /// <summary>
        /// How high the counter should be increased
        /// </summary>
        [Header("Attributes")]
        [SerializeField]
        private int _scoreIncrease = 0;

        [SerializeField]
        private int _healthIncrease = 0;


        public Vector2 Position { get => transform.localPosition; set => transform.localPosition = value; }
        public Vector2 Scale { get => transform.localScale; set => transform.localScale = value; }
        public int ScoreIncrease { get => _scoreIncrease; }
        public int HealthIncrease { get => _healthIncrease; }
        public bool Pulse { get => _pulse;  }

        public static event Action<CollectableOperator> OnCollected;

        public void Setup(CollectableOperator prefab, Vector3 position)
        {
            transform.localScale = prefab.transform.localScale;

            _spriteRenderer.sprite = prefab._spriteRenderer.sprite;
            _spriteRenderer.transform.localScale = prefab._spriteRenderer.transform.localScale;
            _spriteRenderer.color = prefab._spriteRenderer.color;

            position.y += _height.GetRandomFloatBetween();
            transform.position = position;

            _pulse = prefab._pulse;

            // Collider
            _collider.enabled = true;
            _collider.offset = prefab._collider.offset;
            _collider.size = prefab._collider.size;

            // Attributes
            _scoreIncrease = prefab._scoreIncrease;
            _healthIncrease = prefab._healthIncrease;
        }

        public void MakeStart()
        {
            if (_pulse)
                _pulseAnimator.StartAnimation();
        }

        public void Collect()
        {
            // Handle the logic for what happens when the player collects the collectable
            // e.g. play a sound, give the player points, etc.
            // or you can call an event that you can listen in the player script.
            Debug.Log("Collectable Collected!");

            _spriteRenderer.sprite = _collectedSprite;
            StartCoroutine(_spriteFadeAndScaler.FadeAndScale());

            EnvironmentManager.Instance.CollectablesCollected += _scoreIncrease;
            EnvironmentManager.Instance.PlayerOperator.Health += _healthIncrease;
            IngameOverlayUIB.Instance.AddHearts(_healthIncrease);

            OnCollected?.Invoke(this);
        }
    }
}