using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment.Collectable
{

    public class Collectable : MonoBehaviour
    {
        private BoxCollider2D _collider;

        /// <summary>
        /// The amount of collectables collected in the round.
        /// </summary>
        public static int Counter;

        [SerializeField]
        private GameObject _collectableModel;

        [SerializeField]
        private SpriteFadeAndScaler _spriteFadeAndScaler;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private Sprite _collectedSprite;

        public static event Action<Collectable> OnCollected;


        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            ResetCounter();
        }

        private void OnEnable()
        {
            _spriteFadeAndScaler.AfterExecute += Deactivate;
        }

        private void OnDisable()
        {
            _spriteFadeAndScaler.AfterExecute -= Deactivate;
        }

        public void Activate()
        {
            _spriteRenderer.sprite = _sprite;
            _spriteRenderer.transform.localScale = Vector3.one;

            // Enable the collectable's renderer and collider
            _collectableModel.SetActive(true);
            _collider.enabled = true;
        }

        public void Deactivate()
        {
            // Disable the collectable's renderer and collider
            _collectableModel.SetActive(false);
            _collider.enabled = false;
        }

        public void Collect()
        {
            // Handle the logic for what happens when the player collects the collectable
            // e.g. play a sound, give the player points, etc.
            // or you can call an event that you can listen in the player script.
            Debug.Log("Collectable Collected!");

            _spriteRenderer.sprite = _collectedSprite;
            StartCoroutine(_spriteFadeAndScaler.FadeAndScale());

            Counter++;

            OnCollected?.Invoke(this);
        }

        private void ResetCounter()
        {
            Counter = 0;
        }
    }
}