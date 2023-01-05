using Mixin.Language;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class SettingsUIB : UIBuildManager<SettingsUIB>
    {
        private Button ExitButton;
        public SliderInt MusicVolumeSlider { get; set; }
        public SliderInt SoundVolumeSlider { get; set; }
        public DropdownField QualityDropdown { get; set; }
        public DropdownField LanguageDropdown { get; set; }

        public Button SaveButton;

        public static event Action OnExitButtonClicked;
        public static event Action OnSaveButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            ExitButton = _root.Q<Button>("ExitButton");
            MusicVolumeSlider = _root.Q<SliderInt>("MusicVolumeSlider");
            SoundVolumeSlider = _root.Q<SliderInt>("SoundVolumeSlider");
            QualityDropdown = _root.Q<DropdownField>("QualityDropdown");
            LanguageDropdown = _root.Q<DropdownField>("LanguageDropdown");
            SaveButton = _root.Q<Button>("SaveButton");

            QualityDropdown.choices = QualitySettings.names.ToList();

            LanguageDropdown.choices = Enum.GetValues(typeof(Language.Language))
                .Cast<Language.Language>()
                .Select(v => v.ToString())
                .ToList();
        }

        private void OnEnable()
        {
            ExitButton.clicked += () => OnExitButtonClicked?.Invoke();
            SaveButton.clicked += () => OnSaveButtonClicked?.Invoke();
        }

        private void OnDisable()
        {
            ExitButton.clicked -= () => OnExitButtonClicked?.Invoke();
            SaveButton.clicked -= () => OnSaveButtonClicked?.Invoke();
        }

    }
}
