using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove.Scene
{
    public class SceneTransitionManager : Singleton<SceneTransitionManager>
    {
        [SerializeField]
        private float _transitionTime = .5f;

        public void LoadSceneWithTransition(SceneName sceneName, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(LoadSceneWithTransitionAsync(sceneName, loadSceneMode));
        }

        private IEnumerator LoadSceneWithTransitionAsync(SceneName sceneName, LoadSceneMode loadSceneMode)
        {
            // Load new scene
            AsyncOperation operation =
               UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                   sceneName.ToString(), loadSceneMode);

            operation.allowSceneActivation = false;

            // Set transition duration
            TransitionUIB.Instance.TransitionElement.style.transitionDuration =
               new List<TimeValue> { _transitionTime };
            // Add active class
            TransitionUIB.Instance.TransitionElement.AddToClassList("active");

            yield return new WaitForSeconds(_transitionTime);

            /*Now everything is black*/

            while (operation.progress < 0.9f)
                yield return null;

            operation.allowSceneActivation = true;

            // Fade out
            TransitionUIB.Instance.TransitionElement.RemoveFromClassList("active");
        }

        public void UnloadSceneWithTransition(
            SceneName sceneName,
            UnloadSceneOptions unloadSceneMode = UnloadSceneOptions.None)
        {
            StartCoroutine(UnloadSceneWithTransitionAsync(sceneName, unloadSceneMode));
        }

        private IEnumerator UnloadSceneWithTransitionAsync(SceneName sceneName, UnloadSceneOptions loadSceneMode)
        {

            // Set transition duration
            TransitionUIB.Instance.TransitionElement.style.transitionDuration =
               new List<TimeValue> { _transitionTime };
            // Add active class
            TransitionUIB.Instance.TransitionElement.AddToClassList("active");

            yield return new WaitForSeconds(_transitionTime);

            /*Now everything is black*/

            // Unload scene
            AsyncOperation operation =
               UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
                   sceneName.ToString(), loadSceneMode);

            while (operation.progress < 0.9f)
                yield return null;

            // Fade out
            TransitionUIB.Instance.TransitionElement.RemoveFromClassList("active");
        }
    }
}