using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Mixin.TheLastMove.Save;

namespace Mixin.TheLastMove
{
    public class SettingsManager : MonoBehaviour
    {
        private UserSettingsData _data => SaveManager.Instance.UserSettingsData.Data;

        private void OnEnable()
        {
            SettingsUIB.OnSaveButtonClicked += OnSaveButtonClicked;
            SettingsUIB.Instance.MusicVolumeSlider.RegisterValueChangedCallback(UpdateMusicVolume);
            SettingsUIB.Instance.SoundVolumeSlider.RegisterValueChangedCallback(UpdateSoundVolume);
            SettingsUIB.Instance.QualitySlider.RegisterValueChangedCallback(UpdateQuality);
            SettingsUIB.Instance.LanguageDropdown.RegisterValueChangedCallback(UpdateLanguage);
        }

        private void OnSaveButtonClicked()
        {
            Debug.Log(SaveManager.Instance.UserSettingsData.Data);
            Debug.Log(SaveManager.Instance.UserSettingsData);
            _data.MusicVolume = 50;

            SaveManager.Instance.UserSettingsData.Save();
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
