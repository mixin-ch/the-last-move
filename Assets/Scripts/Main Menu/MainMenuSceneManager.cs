using Mixin.Utils;
using Mixin.Utils.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mixin.TheLastMove
{
    public class MainMenuSceneManager : MonoBehaviour
    {
        [SerializeField]
        private AudioTrackSetupSO _themeSong;

        private void Start()
        {
            MainMenuUIB.Instance.GameVersionText.text =
                $"Version {Application.version}";

            AudioManager.Instance.StopAllAudio();
            AudioManager.Instance.PlayTrack(_themeSong);
        }

        private void OnPlayButtonClicked()
        {
            ChangeScene(SceneName.Ingame, LoadSceneMode.Single);
        }

        private void OnSettingsButtonClicked()
        {
            ChangeScene(SceneName.Settings, LoadSceneMode.Additive);
        }

        private void OnAboutButtonClicked()
        {
            ChangeScene(SceneName.About, LoadSceneMode.Additive);
        }

        private void ChangeScene(SceneName sceneName, LoadSceneMode loadSceneMode)
        {
            $"Changing to Scene {sceneName}".LogProgress();
            Utils.SceneManager.Instance.LoadScene(sceneName.ToString(), loadSceneMode);
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

            AudioManager.Instance.StopAllAudio();
        }
    }
}
