using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class PlayerUIOperator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _attackSlash;

        [SerializeField]
        private float _fadeOutTime = 0.5f;

        [SerializeField]
        private Vector3 _targetScale = new Vector3(1, 1, 1);

        // Start is called before the first frame update
        private void OnEnable()
        {
            EnvironmentManager.Instance.PlayerOperator.OnPlayerAttackEvent += PlayerOperator_OnPlayerAttackEvent;
            _attackSlash.enabled = false;
        }

        private void PlayerOperator_OnPlayerAttackEvent()
        {
            // Display the sprite
            _attackSlash.enabled = true;

            // Fade out the sprite over time
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            // Fade out the sprite
            float elapsedTime = 0;
            Color c = _attackSlash.color;
            while (elapsedTime < _fadeOutTime)
            {
                elapsedTime += Time.deltaTime;
                c.a = 1 - (elapsedTime / _fadeOutTime);
                _attackSlash.color = c;
                _attackSlash.gameObject.transform.localScale =
                    Vector3.Lerp(transform.localScale, _targetScale, elapsedTime / _fadeOutTime);
                yield return null;
            }

            // Disable the sprite renderer when the fade is complete
            _attackSlash.enabled = false;
        }
    }
}
