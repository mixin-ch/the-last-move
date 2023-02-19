using Mixin.TheLastMove.Environment;
using Mixin.Utils.Audio;
using System.Collections;
using UnityEngine;

namespace Mixin.TheLastMove.Player
{
    public class PlayerSlashOperator : MonoBehaviour
    {
        [SerializeField]
        private bool _canBoost;
        [SerializeField]
        private float _boostIntensity;
        [SerializeField]
        private AudioTrackSetupSO _boostSound;

        [SerializeField]
        private SpriteRenderer _attackSlash;
        [SerializeField]
        private Transform _physical;

        [SerializeField]
        private Vector2 _startSize;
        [SerializeField]
        private Vector2 _endSize;
        [SerializeField]
        private float _endDisplacement;
        [SerializeField]
        private float _stayDuration;
        [SerializeField]
        private float _fadeOutDuration;

        private float _elapsedTime;

        // Start is called before the first frame update
        private void OnEnable()
        {
            InputManager.OnPlayerAttack += PlayerOperator_OnPlayerAttackEvent;
            _attackSlash.enabled = false;
            _physical.gameObject.SetActive(false);
        }

        private void PlayerOperator_OnPlayerAttackEvent()
        {
            _physical.transform.localPosition = Vector3.zero;
            _physical.transform.localScale = _startSize;
            _attackSlash.color = Color.white;
            _elapsedTime = 0;
            _physical.gameObject.SetActive(true);

            // Display the sprite
            _attackSlash.enabled = true;

            // Fade out the sprite over time
            StartCoroutine(ProcessSlash());
        }

        IEnumerator ProcessSlash()
        {
            Color color = Color.white;

            while (_elapsedTime < _stayDuration + _fadeOutDuration)
            {
                if (_elapsedTime > _stayDuration)
                {
                    float fadeTime = _elapsedTime - _stayDuration;
                    color.a = 1 - (fadeTime / _fadeOutDuration);
                    _attackSlash.color = color;
                }

                float fraction = _elapsedTime / (_stayDuration + _fadeOutDuration);

                _physical.transform.localScale = Vector2.Lerp(_startSize, _endSize, fraction);
                _physical.transform.localPosition = Vector2.right * Mathf.Lerp(0, _endDisplacement, fraction);

                yield return null;

                _elapsedTime += Time.deltaTime;
            }

            // Disable the sprite renderer when the fade is complete
            _attackSlash.enabled = false;
            _physical.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Obstacle"))
                return;

            ObstacleOperator @operator = collision.gameObject.GetComponent<ObstacleOperator>();

            if (@operator == null)
                return;

            if (@operator.Killable)
            {
                @operator.Kill();
                return;
            }

            if (!_canBoost)
                return;

            EnvironmentManager.Instance.PlayerOperator.Boost(_boostIntensity);
            AudioManager.Instance.PlayTrack(_boostSound);
        }
    }
}
