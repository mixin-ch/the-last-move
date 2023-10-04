using Mixin.TheLastMove.Save;
using Mixin.TheLastMove.Scene;
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

        private bool _isPlayButtonEnabled = true;

        private void Start()
        {
            MainMenuUIB.Instance.GameVersionText.text =
                $"Version {Application.version}";

            AudioManager.Instance.StopAllAudio();
            AudioManager.Instance.PlayTrack(_themeSong);
        }

        private void OnPlayButtonClicked()
        {
            if (_isPlayButtonEnabled)
            {
                _isPlayButtonEnabled = false;
                if (SaveManager.Instance.IngameData.Data.Highscore==0) {
                    ChangeScene(SceneName.Tutorial, LoadSceneMode.Single);
                }
                else
                {
                    ChangeScene(SceneName.Ingame, LoadSceneMode.Single);
                }
            }
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
            SceneTransitionManager.Instance.LoadSceneWithTransition(sceneName, loadSceneMode);
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
