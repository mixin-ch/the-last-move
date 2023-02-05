using Mixin.TheLastMove.Sound;
using System;
using System.Collections;
using UnityEngine;

namespace Mixin.TheLastMove.Player
{
    public class PlayerAnimationOperator : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _sprites; // array of sprites

        [SerializeField]
        private float _changeTime; // change time in seconds

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private int _currentSprite = 0; // index of the current sprite

        public event Action OnPlayWalkSound;

        void Start()
        {
            StartCoroutine(ChangeSprite()); // start coroutine to change sprites
        }

        private IEnumerator ChangeSprite()
        {
            int soundPlayInterval = _sprites.Length / 2;

            while (true) // loop indefinitely
            {
                yield return new WaitForSeconds(_changeTime); // wait for changeTime seconds
                _currentSprite = (_currentSprite + 1) % _sprites.Length; // move to the next sprite
                _spriteRenderer.sprite = _sprites[_currentSprite]; // change the sprite

                if (_currentSprite % soundPlayInterval == 0) // play the sound only on specific indices
                {
                    PlayWalkSound();
                }
            }
        }

        private void PlayWalkSound()
        {
            IngameSoundManager.Instance.PlaySound(SoundType.Walk);
        }
    }
}
