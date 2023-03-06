using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Player
{
    public class PlayerOperator : MonoBehaviour
    {
        public static event Action<PlayerOperator> OnPlayerDeathEvent;
        public static event Action<PlayerOperator> OnPlayerTakeDamageEvent;
        public static event Action<PlayerOperator> OnPlayerLanded;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private Transform _stretcherTransform;
        [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private PlayerAnimationOperator _playerAnimator;
        [SerializeField]
        private PlayerSlashOperator _meleeSlash;
        [SerializeField]
        private PlayerSlashOperator _rangedSlash;

        [SerializeField]
        private int _startHealth;
        [SerializeField]
        private Vector2 _startPosition;
        [SerializeField]
        private List<float> _jumpList;

        private const float _stretchCatchup = 300f;
        private const float _stretchCushion = 10f;
        private const float _stretchExtent = 1f;

        private const float _gravity = 1f;
        private const float _jumpVelocityBreak = 5f;
        private const float _immunityDuration = 1;

        private const float _landDuration = 0.15f;
        private const float _attackDuration = 0.15f;

        private float Hectic => EnvironmentManager.Instance.Hectic;

        private float Gravity => _gravity * Hectic;

        private bool _active;

        private float _health;

        private float _immunityTime;

        private float _pausedVelocity;

        private float _dragPointOffset;
        private float _dragPointVelocity;

        private List<float> _remainingJumpList = new List<float>();
        private bool _isJumping;

        private PlayerSpriteState _playerSpriteState;
        private float _spriteTime;

        private bool HasJump => _remainingJumpList.Count > 0;

        public bool CanJump => HasJump && !_isJumping;

        public PlayerSlashOperator MeleeSlash { get => _meleeSlash; }
        public PlayerSlashOperator RangedSlash { get => _rangedSlash; }

        public Vector2 Position => transform.position;
        public float Health { get => _health; set => _health = value; }
        public PlayerSpriteState PlayerSpriteState { get => _playerSpriteState; }

        private void OnEnable()
        {
            InputManager.OnPlayerJump += Jump;
            InputManager.OnPlayerAttack += Attack;
        }

        private void OnDisable()
        {
            InputManager.OnPlayerJump -= Jump;
            InputManager.OnPlayerAttack -= Attack;
        }

        public void Tick(float time)
        {
            if (!_active)
                return;

            _immunityTime = (_immunityTime - time).LowerBound(0);
            _isJumping = _isJumping && InputManager.Instance.IsPressingJumpButton && _rigidbody.velocity.y > 0;

            if (!_isJumping && _rigidbody.velocity.y > 0)
                _rigidbody.velocity = Vector2.up * Mathf.Lerp(_rigidbody.velocity.y, 0, _jumpVelocityBreak * time);

            if (_playerSpriteState == PlayerSpriteState.Attack)
            {
                _spriteTime += time;

                if (_spriteTime >= _attackDuration)
                    _playerSpriteState = PlayerSpriteState.Walk;
            }

            if (_playerSpriteState != PlayerSpriteState.Attack)
            {
                if (_rigidbody.velocity.y > 0)
                    _playerSpriteState = PlayerSpriteState.Jump;
                else if (_rigidbody.velocity.y < 0)
                    _playerSpriteState = PlayerSpriteState.Fall;
                else if (_rigidbody.velocity.y == 0)
                {
                    if (_playerSpriteState == PlayerSpriteState.Land)
                    {
                        _spriteTime += time;

                        if (_spriteTime >= _landDuration)
                            _playerSpriteState = PlayerSpriteState.Walk;
                    }
                    else
                    {
                        _playerSpriteState = PlayerSpriteState.Walk;
                    }
                }
            }

            _rigidbody.gravityScale = Gravity;

            ProcessStretch(time);

            if (Position.y < -5)
                Die();
        }

        private void ProcessStretch(float time)
        {
            _dragPointOffset = _dragPointOffset + (_dragPointVelocity - _rigidbody.velocity.y) * time;
            _dragPointOffset = Mathf.Lerp(_dragPointOffset, 0, _stretchCushion * time);
            _dragPointVelocity -= _dragPointOffset * _stretchCatchup * time;

            float stretch = (_dragPointOffset * _stretchExtent).AmplifierToMultiplier();
            _stretcherTransform.localScale = new Vector3(1 / stretch, stretch, 1);
        }

        public void PauseRefresh()
        {
            if (!_active)
                return;

            RefreshVelocity();
        }

        private void Jump()
        {
            _isJumping = true;
            _rigidbody.velocity = Vector2.up * _remainingJumpList[0] * Mathf.Sqrt(Hectic);
            _remainingJumpList.RemoveAt(0);
            _playerSpriteState = PlayerSpriteState.Jump;
        }

        public void Boost(float intensity)
        {
            if (!_active)
                return;

            _isJumping = true;
            _rigidbody.velocity = Vector2.up * intensity * Mathf.Sqrt(Hectic);
            _playerSpriteState = PlayerSpriteState.Jump;
            _remainingJumpList.Clear();
            _remainingJumpList.AddRange(_jumpList);
        }

        private void Attack()
        {
            _playerSpriteState = PlayerSpriteState.Attack;
            _spriteTime = 0;
        }

        private void TakeDamage()
        {
            _immunityTime = _immunityDuration;
            _health--;
            OnPlayerTakeDamageEvent?.Invoke(this);

            if (_health <= 0)
                Die();
        }

        public void StartPlayer()
        {
            ResetState();
            gameObject.SetActive(true);
            _playerAnimator.AnimationStart();
            _active = true;
        }

        public void SoftStartPlayer()
        {
            ResetState();
            _health = 1;
            gameObject.SetActive(true);
            _playerAnimator.AnimationStart();
            _active = true;
        }

        private void Die()
        {
            _active = false;
            _playerAnimator.AnimationStop();
            gameObject.SetActive(false);
            OnPlayerDeathEvent?.Invoke(this);
        }

        private void ResetState()
        {
            transform.position = _startPosition;
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            _stretcherTransform.localScale = Vector2.one;

            _health = _startHealth;
            _pausedVelocity = 0;
            _remainingJumpList.Clear();
            _isJumping = false;
            _dragPointOffset = 1;
            _playerSpriteState = PlayerSpriteState.Walk;
        }

        private void RefreshVelocity()
        {
            if (EnvironmentManager.Instance.Paused)
            {
                _rigidbody.gravityScale = 0;
                _pausedVelocity = _rigidbody.velocity.y;
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.gravityScale = Gravity;
                _rigidbody.velocity = Vector2.up * _pausedVelocity;
                _pausedVelocity = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled && collision.collider.bounds.center.y < collision.otherCollider.bounds.center.y)
            {
                _remainingJumpList.Clear();
                _remainingJumpList.AddRange(_jumpList);

                if (_playerSpriteState == PlayerSpriteState.Fall)
                {
                    OnPlayerLanded?.Invoke(this);
                    _playerSpriteState = PlayerSpriteState.Land;
                    _spriteTime = 0;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Obstacle") && _immunityTime <= 0)
            {
                ObstacleOperator @operator = collision.gameObject.GetComponent<ObstacleOperator>();

                if ((_collider.bounds.center.y >= collision.bounds.min.y && @operator.UpHarm)
                      || (_collider.bounds.center.y <= collision.bounds.max.y && @operator.DownHarm))
                    TakeDamage();
            }

            if (collision.gameObject.CompareTag("Collectable"))
                collision.gameObject.GetComponent<CollectableOperator>().Collect();
        }
    }
}
