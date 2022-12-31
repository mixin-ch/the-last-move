using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class MainMenuSceneManager : MonoBehaviour
    {
        private void OnPlayButtonClicked()
        {
            ChangeScene(SceneName.Ingame);
        }

        private void OnSettingsButtonClicked()
        {
            ChangeScene(SceneName.Settings);
        }

        private void OnAboutButtonClicked()
        {
            ChangeScene(SceneName.About);
        }

        private void ChangeScene(SceneName sceneName)
        {
            $"Changing to Scene {sceneName}".LogProgress();
            SceneManager.Instance.LoadScene(sceneName.ToString());
        }

        private void OnEnable()
        {
            MainMenuUIB.OnPlayButtonClicked += OnPlayButtonClicked;
            MainMenuUIB.OnSettingsButtonClicked += OnSettingsButtonClicked;
            MainMenuUIB.OnAboutButtonClicked += OnAboutButtonClicked;
        }

        private void OnDisable()
        {
            MainMenuUIB.OnPlayButtonClicked -= OnPlayButtonClicked;
            MainMenuUIB.OnSettingsButtonClicked -= OnSettingsButtonClicked;
            MainMenuUIB.OnAboutButtonClicked -= OnAboutButtonClicked;
        }
    }
}
