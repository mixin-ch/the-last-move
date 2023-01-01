using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class PlayerOperator : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private Transform _imageTransform;

        private const float _gravity = 10f;
        private const float _jumpVelocity = 4f;
        private const int _jumps = 2;
        private const float _jumpTime = 0.5f;

        private float Hectic => EnvironmentManager.Instance.Hectic;

        private float Gravity => _gravity * Hectic;
        private float JumpVelocity => _jumpVelocity * Mathf.Sqrt(Hectic);
        private int Jumps => _jumps;
        private float JumpTime => _jumpTime / Mathf.Sqrt(Hectic);

        private float _velocity;
        private int _remainingJumps;
        private bool _isJumping;
        private float _jumpTimeRemaining;

        private bool HasJump => _remainingJumps > 0;

        public void Setup()
        {
            ResetState();
        }

        public void Tick(float time)
        {
            _isJumping = _isJumping && InputManager.Instance.IsPressingJumpButton;

            if (!_isJumping)
                _jumpTimeRemaining = 0;
            else
                _jumpTimeRemaining = (_jumpTimeRemaining - time).LowerBound(0);

            if (_jumpTimeRemaining <= 0)
            {
                _isJumping = false;
                _velocity += -Gravity * time;
            }

            RefreshVelocity();
        }

        public void Pause()
        {
            RefreshVelocity();
        }

        public void TryJump()
        {
            if (!HasJump)
                return;
            if (_isJumping)
                return;

            _isJumping = true;
            _remainingJumps--;
            _jumpTimeRemaining = JumpTime;
            _velocity = JumpVelocity;
            RefreshVelocity();
        }

        public void ResetState()
        {
            transform.position = Vector2.up * 3;
            _rigidbody.velocity = Vector2.zero;
            _imageTransform.localScale = Vector2.one;
            _velocity = 0;
            _remainingJumps = 0;
            _isJumping = false;
            _jumpTimeRemaining = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
           if(collision.enabled)
            {
                _velocity = 0;
                _jumpTimeRemaining = 0;
                _remainingJumps = Jumps;
                _isJumping = false;
                RefreshVelocity();
            }
        }

        private void RefreshVelocity()
        {
            if (EnvironmentManager.Instance.Paused)
                _rigidbody.velocity = Vector2.zero;
            else
                _rigidbody.velocity = Vector2.up * _velocity;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;
    }
}
