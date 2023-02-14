using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class WorldTransitionManager : MonoBehaviour
    {
        // Serialized field for the sprite renderer component, which will be used to adjust the opacity of the sprite
        [SerializeField]
        private SpriteRenderer _sprite;

        // Duration of the fade in and out
        private float _duration = 1.5f;

        public string Log { get; set; }

        public static event Action<BiomeSO> OnBiomeChangeTransition;

        private void Start()
        {
            _sprite.gameObject.SetActive(false);
            SetOpacity(0);
            EnvironmentManager.OnBiomeChanged += MakeTransition;
        }

        private void OnDestroy()
        {
            EnvironmentManager.OnBiomeChanged -= MakeTransition;
        }

        private void MakeTransition(BiomeSO biome)
        {
            StartCoroutine(MakeTransitionCoroutine(biome));
        }

        // Public function that starts the fade in and out process
        private IEnumerator MakeTransitionCoroutine(BiomeSO biome)
        {
            Log = $"Making transition";

            _sprite.gameObject.SetActive(true);

            // Start the fade in coroutine
            StartCoroutine(FadeToOpacity(1f));

            // Wait for the fade in to finish
            yield return new WaitForSeconds(_duration * 1.5f);

            OnBiomeChangeTransition?.Invoke(biome);

            // Start the fade out coroutine
            StartCoroutine(FadeToOpacity(0f));

            // Wait for the fade in to finish
            yield return new WaitForSeconds(_duration);

            _sprite.gameObject.SetActive(false);

            Log = $"Finished transition";
        }

        // Coroutine that smoothly adjusts the opacity of the sprite over the course of _duration
        private IEnumerator FadeToOpacity(float targetOpacity)
        {
            // Start time and initial opacity of the sprite
            float currentTime = 0f;
            float initialOpacity = _sprite.color.a;

            // While the current time is less than the duration
            while (currentTime < _duration)
            {
                // Increase the current time by the delta time
                currentTime += Time.deltaTime;
                // Calculate the new opacity using a Lerp function
                float newOpacity = Mathf.Lerp(initialOpacity, targetOpacity, currentTime / _duration);
                // Set the color of the sprite to the new opacity
                SetOpacity(newOpacity);
                // Wait for one frame
                yield return null;
            }
        }

        private void SetOpacity(float newOpacity)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, newOpacity);
        }
    }
}
