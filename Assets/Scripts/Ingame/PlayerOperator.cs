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
        private const float _jumpVelocity = 8f;
        private const float _drag = 1f;
        private const float _squishExtent = 1f;

        private float _velocity;
        private float _dragVelocity;
        private bool _hasJump;

        public void Setup()
        {
            ResetState();
        }

        public void Tick(float time)
        {
            _velocity += -_gravity * time;
            RefreshVelocity();

            //float catchup = time * (1 - _drag / (1 + _drag));
            //_dragVelocity = Mathf.Lerp(_dragVelocity, _velocity, catchup);

            //float squish = 1 + Mathf.Abs(_dragVelocity - _velocity) * _squishExtent;
            //float stretchX = squish;
            //float stretchY = 1 / stretchX;
            //_imageTransform.localScale = new Vector2(stretchX, stretchY);
        }

        public void Pause(bool pause)
        {
            if (pause)
                _rigidbody.velocity = Vector2.zero;
            else
                _rigidbody.velocity = Vector2.up * _velocity;

            RefreshVelocity();
        }

        public void TryJump()
        {
            if (!_hasJump)
                return;

            _hasJump = false;
            _velocity = _jumpVelocity;
            RefreshVelocity();
        }

        public void ResetState()
        {
            transform.position = Vector2.up * 3;
            _rigidbody.velocity = Vector2.zero;
            _imageTransform.localScale = Vector2.one;
            _velocity = 0;
            _dragVelocity = 0;
            _hasJump = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _hasJump = true;
        }

        private void RefreshVelocity()
        {
            _rigidbody.velocity = Vector2.up * _velocity;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public Vector2 Position => transform.position;
    }
}
