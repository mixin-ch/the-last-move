using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using System;
using Mixin.TheLastMove.Scene;
using UnityEngine.SceneManagement;

namespace Mixin.TheLastMove
{
    public class SettingsSceneManager : MonoBehaviour
    {
        private void Start()
        {

        }

        private void OnEnable()
        {
            SettingsUIB.OnExitButtonClicked += OnExitButtonClicked;
        }

        private void OnDisable()
        {
            SettingsUIB.OnExitButtonClicked -= OnExitButtonClicked;
        }

        public void LoadSettings()
        {

        }

        private void SaveSettings()
        {

        }

        private void OnExitButtonClicked()
        {
            SceneTransitionManager.Instance.UnloadSceneWithTransition(SceneName.Settings);
        }
    }
}
