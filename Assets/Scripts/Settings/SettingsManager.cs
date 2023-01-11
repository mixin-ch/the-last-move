using System;
using UnityEngine;
using UnityEngine.UIElements;
using Mixin.TheLastMove.Save;
using Mixin.Utils;

namespace Mixin.TheLastMove.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private UserSettingsData _data => SaveManager.Instance.UserSettingsData.Data;
        private SettingsUIB _uib => SettingsUIB.Instance;

        private void Start()
        {
            _uib.MusicVolumeSlider.value = _data.MusicVolume;
            _uib.SoundVolumeSlider.value = _data.SoundVolume;
            _uib.QualityDropdown.value = QualitySettings.names[QualitySettings.GetQualityLevel()];
            _uib.LanguageDropdown.value = _data.Language.ToString();

            _uib.SaveButton.clicked += OnSaveButtonClicked;
            _uib.MusicVolumeSlider.RegisterValueChangedCallback(UpdateMusicVolume);
            _uib.SoundVolumeSlider.RegisterValueChangedCallback(UpdateSoundVolume);
            _uib.QualityDropdown.RegisterValueChangedCallback(UpdateQuality);
            _uib.LanguageDropdown.RegisterValueChangedCallback(UpdateLanguage);
        }

        private void OnSaveButtonClicked()
        {
            QualitySettings.SetQualityLevel(_data.Quality);
            SaveManager.Instance.UserSettingsData.Save();
        }

        private void UpdateMusicVolume(ChangeEvent<int> evt)
        {
            _data.MusicVolume = evt.newValue;
        }

        private void UpdateSoundVolume(ChangeEvent<int> evt)
        {
            _data.SoundVolume = evt.newValue;
        }

        private void UpdateQuality(ChangeEvent<string> evt)
        {
            _data.Quality = Array.IndexOf(QualitySettings.names, evt.newValue);
        }

        private void UpdateLanguage(ChangeEvent<string> evt)
        {
            if (Enum.TryParse<Language.Language>(evt.newValue, out Language.Language language))
                _data.Language = language;
        }
    }
}
