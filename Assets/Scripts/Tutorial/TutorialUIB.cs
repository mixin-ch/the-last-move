using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class TutorialUIB : UIBuildManager<TutorialUIB>
{
    public VisualElement Background;

    protected override void Awake()
    {
        base.Awake();

        Background = _root.Q<VisualElement>("Background");
    }

    public void ChangeBackgroundImage(Sprite sprite)
    {
        Background.style.backgroundImage = new StyleBackground(sprite);
    }
}
