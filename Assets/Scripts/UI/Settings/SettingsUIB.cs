using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class SettingsUIB : UIBuildCollector<SettingsUIB>
    {
        public Button ExitButton { get; set; }
        public SliderInt MusicVolumeSlider { get; set; }
        public SliderInt SoundVolumeSlider { get; set; }
        public SliderInt QualitySlider { get; set; }
        public DropdownField LanguageDropdown { get; set; }

        public static event Action OnExitButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            ExitButton = _root.Q<Button>("ExitButton");
            MusicVolumeSlider = _root.Q<SliderInt>("MusicVolumeSlider");
            SoundVolumeSlider = _root.Q<SliderInt>("SoundVolumeSlider");
            QualitySlider = _root.Q<SliderInt>("QualitySlider");
            LanguageDropdown = _root.Q<DropdownField>("LanguageDropdown");
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

        private void OnEnable()
        {
            ExitButton.clicked += () => OnExitButtonClicked?.Invoke();
            MusicVolumeSlider.RegisterValueChangedCallback(UpdateMusicVolume);
            SoundVolumeSlider.RegisterValueChangedCallback(UpdateSoundVolume);
            QualitySlider.RegisterValueChangedCallback(UpdateQuality);
            LanguageDropdown.RegisterValueChangedCallback(UpdateLanguage);
        }

        private void OnDisable()
        {
            ExitButton.clicked -= () => OnExitButtonClicked?.Invoke();
            MusicVolumeSlider.UnregisterValueChangedCallback(UpdateMusicVolume);
            SoundVolumeSlider.UnregisterValueChangedCallback(UpdateSoundVolume);
            QualitySlider.UnregisterValueChangedCallback(UpdateQuality);
            LanguageDropdown.UnregisterValueChangedCallback(UpdateLanguage);
        }

    }
}
