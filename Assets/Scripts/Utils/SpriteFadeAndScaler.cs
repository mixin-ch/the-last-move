using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFadeAndScaler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    private float _scaleFactor = 1.01f;

    public event Action BeforeExecute;
    public event Action AfterExecute;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FadeAndScale()
    {
        return FadeAndScale(_fadeTime, _scaleFactor);
    }

    public IEnumerator FadeAndScale(float fadeTime, float scaleFactor)
    {
        float elapsedTime = 0f;
        Color originalColor = _spriteRenderer.color;

        BeforeExecute?.Invoke();

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(originalColor, Color.clear, elapsedTime / fadeTime);
            _spriteRenderer.transform.localScale *= scaleFactor;
            yield return null;
        }

        AfterExecute?.Invoke();
    }
}
