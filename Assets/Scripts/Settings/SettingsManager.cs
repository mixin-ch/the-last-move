using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class SettingsManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SettingsUIB.OnSaveButtonClicked += OnSaveButtonClicked;
            SettingsUIB.Instance.MusicVolumeSlider.RegisterValueChangedCallback(UpdateMusicVolume);
            SettingsUIB.Instance.SoundVolumeSlider.RegisterValueChangedCallback(UpdateSoundVolume);
            SettingsUIB.Instance.QualitySlider.RegisterValueChangedCallback(UpdateQuality);
            SettingsUIB.Instance.LanguageDropdown.RegisterValueChangedCallback(UpdateLanguage);
        }

        private void OnDisable()
        {
            SettingsUIB.OnSaveButtonClicked -= OnSaveButtonClicked;
            SettingsUIB.Instance.MusicVolumeSlider.UnregisterValueChangedCallback(UpdateMusicVolume);
            SettingsUIB.Instance.SoundVolumeSlider.UnregisterValueChangedCallback(UpdateSoundVolume);
            SettingsUIB.Instance.QualitySlider.UnregisterValueChangedCallback(UpdateQuality);
            SettingsUIB.Instance.LanguageDropdown.UnregisterValueChangedCallback(UpdateLanguage);
        }

        private void OnSaveButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void UpdateMusicVolume(ChangeEvent<int> evt)
        {
            throw new NotImplementedException();
        }

        private void UpdateSoundVolume(ChangeEvent<int> evt)
        {
            throw new NotImplementedException();
        }

        private void UpdateQuality(ChangeEvent<int> evt)
        {
            throw new NotImplementedException();
        }

        private void UpdateLanguage(ChangeEvent<string> evt)
        {
            throw new NotImplementedException();
        }
    }
}
