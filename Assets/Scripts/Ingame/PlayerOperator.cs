using Mixin.Utils;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class PlayerOperator : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rigidbody;

        private const float _gravity = 10f;
        private const float _jumpVelocity = 8f;

        private float _velocity;
        private bool _hasJump;

        public void Setup()
        {
            ResetState();
        }

        public void Tick(float time)
        {
            _rigidbody.velocity += Vector2.down * _gravity * time;
        }

        public void Pause(bool pause)
        {
            if (pause)
            {
                _velocity = _rigidbody.velocity.y;
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.velocity = Vector2.up * _velocity;
            }
        }

        public void TryJump()
        {
            if (!_hasJump)
                return;

            _hasJump = false;
            _rigidbody.velocity = Vector2.up * _jumpVelocity;
        }

        public void ResetState()
        {
            _hasJump = false;
            transform.position = Vector2.up * 3;
            _rigidbody.velocity = Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _hasJump = true;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;
    }
}
