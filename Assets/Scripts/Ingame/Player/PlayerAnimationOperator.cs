using Mixin.TheLastMove.Environment;
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

        [SerializeField]
        private Sprite _jump;

        [SerializeField]
        private Sprite _fall;

        [SerializeField]
        private Sprite _land;

        [SerializeField]
        private Sprite _attack;

        private int _currentSprite = 0; // index of the current sprite

        private PlayerOperator _playerOperator => EnvironmentManager.Instance.PlayerOperator;

        public event Action OnPlayWalkSound;

        private void OnEnable()
        {
            InputManager.OnPlayerJump += InputManager_OnPlayerJump;
            _playerOperator.OnPlayerLanded += _playerOperator_OnPlayerLanded;
            StartCoroutine(ChangeSprite()); // start coroutine to change sprites
        }

        private void OnDisable()
        {
            InputManager.OnPlayerJump -= InputManager_OnPlayerJump;
            _playerOperator.OnPlayerLanded -= _playerOperator_OnPlayerLanded;
            StopAllCoroutines();
        }

        private IEnumerator ChangeSprite()
        {
            int soundPlayInterval = _sprites.Length / 2;

            while (true) // loop indefinitely
            {
                yield return new WaitForSeconds(_changeTime); // wait for changeTime seconds

                switch (_playerOperator.PlayerSpriteState)
                {
                    case PlayerSpriteState.Walk:
                        _currentSprite = (_currentSprite + 1) % _sprites.Length; // move to the next sprite
                        _spriteRenderer.sprite = _sprites[_currentSprite]; // change the sprite

                        if (_currentSprite % soundPlayInterval == 0) // play the sound only on specific indices
                        {
                            PlayWalkSound();
                        }
                        break;
                    case PlayerSpriteState.Jump:
                        _spriteRenderer.sprite = _jump;
                        break;
                    case PlayerSpriteState.Fall:
                        _spriteRenderer.sprite = _fall;
                        break;
                    case PlayerSpriteState.Land:
                        _spriteRenderer.sprite = _land;
                        break;
                    case PlayerSpriteState.Attack:
                        _spriteRenderer.sprite = _attack;
                        break;
                    default:
                        break;
                }
            }
        }

        private void PlayWalkSound()
        {
            IngameSoundManager.Instance.PlaySound(SoundType.Walk);
        }

        private void InputManager_OnPlayerJump()
        {
            //_spriteRenderer.sprite = _jump;
        }

        private void _playerOperator_OnPlayerLanded()
        {
            //StartCoroutine(ChangeSprite());
        }
    }
}
