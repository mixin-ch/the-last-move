using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using UnityEngine.Audio;
using Mixin.TheLastMove.Settings;
using Mixin.TheLastMove.Save;
using Mixin.Language;

namespace Mixin.TheLastMove
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        [SerializeField]
        private AudioMixer _audioMixer;

        private UserSettingsData _settingsData => SaveManager.Instance.UserSettingsData.Data;

        public AudioMixer AudioMixer { get => _audioMixer; }

        private void Start()
        {
            // Load User Game Data

            // Load User Settings
            Application.targetFrameRate = 60;

            int musicVolume = _settingsData.MusicVolume;
            SettingsManager.SetVolume("MusicVolume", musicVolume);

            int soundVolume = _settingsData.SoundVolume;
            SettingsManager.SetVolume("SoundVolume", soundVolume);

            LanguageManager.Instance.SelectedLanguage = _settingsData.Language;
        }
    }
}
