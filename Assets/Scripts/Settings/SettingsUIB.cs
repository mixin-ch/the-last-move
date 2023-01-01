using Mixin.Utils;
using System;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class SettingsUIB : UIBuildManager<SettingsUIB>
    {
        private Button ExitButton;
        public SliderInt MusicVolumeSlider { get; set; }
        public SliderInt SoundVolumeSlider { get; set; }
        public SliderInt QualitySlider { get; set; }
        public DropdownField LanguageDropdown { get; set; }

        private Button SaveButton;

        public static event Action OnExitButtonClicked;
        public static event Action OnSaveButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            ExitButton = _root.Q<Button>("ExitButton");
            MusicVolumeSlider = _root.Q<SliderInt>("MusicVolumeSlider");
            SoundVolumeSlider = _root.Q<SliderInt>("SoundVolumeSlider");
            QualitySlider = _root.Q<SliderInt>("QualitySlider");
            LanguageDropdown = _root.Q<DropdownField>("LanguageDropdown");
            SaveButton = _root.Q<Button>("SaveButton");
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
