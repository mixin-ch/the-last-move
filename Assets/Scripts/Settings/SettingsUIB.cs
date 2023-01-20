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
        private Label PageTitle;
        [SerializeField]
        private LanguageTextSO _pageTitleLanguage;

        private Button ExitButton;

        public CustomSlider MusicVolumeSlider { get; set; }
        [SerializeField]
        private LanguageTextSO _musicVolumeLanguage;

        public CustomSlider SoundVolumeSlider { get; set; }
        [SerializeField]
        private LanguageTextSO _soundVolumeLanguage;

        /* Language Buttons */
        public Button EnglishButton { get; set; }
        public Button GermanButton { get; set; }
        public Button SwissGermanButton { get; set; }
        public Button FrenchButton { get; set; }

        public Button SaveButton;
        [SerializeField]
        private LanguageTextSO _saveLanguage;

        public static event Action OnExitButtonClicked;
        public static event Action OnSaveButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

        public void Init()
        {
            PageTitle = _root.Q<Label>("PageTitle");
            PageTitle.text = _pageTitleLanguage.GetText();

            ExitButton = _root.Q<Button>("ExitButton");

            MusicVolumeSlider = _root.Q<CustomSlider>("MusicVolumeSlider");
            MusicVolumeSlider.label = _musicVolumeLanguage.GetText();

            SoundVolumeSlider = _root.Q<CustomSlider>("SoundVolumeSlider");
            SoundVolumeSlider.label = _soundVolumeLanguage.GetText();

            EnglishButton = _root.Q<Button>("EnglishButton");
            GermanButton = _root.Q<Button>("GermanButton");
            SwissGermanButton = _root.Q<Button>("SwissGermanButton");
            FrenchButton = _root.Q<Button>("FrenchButton");

            SaveButton = _root.Q<Button>("SaveButton");
            SaveButton.text = _saveLanguage.GetText();
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
