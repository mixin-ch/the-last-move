using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{
    [SerializeField]
    private float _changeTime = 0.1f;

    [SerializeField]
    private AnimationSequenceImage[] _animationSequenceImages;

    // Start is called before the first frame update
    void Start()
    {
        
    }


}

[System.Serializable]
public class AnimationSequenceImage
{
    [SerializeField]
    private Sprite _sprite;

    [SerializeField]
    private float _changeTimeMultiplier = 1f;

    public Sprite Sprite { get => _sprite; }
    public float ChangeTimeMultiplier { get => _changeTimeMultiplier; set => _changeTimeMultiplier = value; }
}