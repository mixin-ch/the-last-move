using Mixin.Utils;
using System.Collections;
using UnityEngine;

public class PulseAnimator : MonoBehaviour
{
    [SerializeField]
    private float _period = 1;
    [SerializeField]
    private float _extent = 1;

    private bool _active;
    private float _time;

    public void StartAnimation()
    {
        _time = 0;
        StopAllCoroutines();
        _active = true;
        StartCoroutine(Pulsate());
    }

    public void StopAnimation()
    {
        _active = false;
        StopAllCoroutines();
    }

    private IEnumerator Pulsate()
    {
        while (_active)
        {
            yield return null;
            _time = (_time + Time.deltaTime).Modulo(_period);
            float size = 1 + _extent * Mathf.Sin(_time / _period * Mathf.PI * 2).LowerBound(0);
            transform.localScale = new Vector2(size, size);
        }
    }
}