using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using UnityEngine.UIElements;

public class TransitionUIB : UIBuildManager<TransitionUIB>
{
    public VisualElement TransitionElement { get; set; }

    protected override void Awake()
    {
        base.Awake();

        TransitionElement = _root.Q<VisualElement>("TransitionElement");
    }

    /*private void Start()
    {
        TransitionElement.AddToClassList("active");
        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {
        yield return new WaitForSeconds(2);
        TransitionElement.RemoveFromClassList("active");
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Transition");
    }*/
}
