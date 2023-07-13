using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainManagerPrefab;

    private static bool _isInited;

    public static ContextLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        if (_isInited)
            return;

        Instantiate(_mainManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        _isInited = true;
    }
}
