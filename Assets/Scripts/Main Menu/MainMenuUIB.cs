using Mixin.Language;
using Mixin.TheLastMove.Sound;
using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class MainMenuUIB : UIBuildManager<MainMenuUIB>
    {
        public Button PlayButton { get; set; }
        public Button SettingsButton { get; set; }
        public Button AboutButton { get; set; }
        public Label TapToPlay { get; set; }
        [SerializeField]
        private LanguageTextSO _tapToPlayLanguage;
        public Label GameVersionText { get; set; }

        private List<Button> _buttonList = new List<Button>();

        public static event Action OnPlayButtonClicked;
        public static event Action OnSettingsButtonClicked;
        public static event Action OnAboutButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            PlayButton = _root.Q<Button>("PlayButton");
            _buttonList.Add(PlayButton);

            SettingsButton = _root.Q<Button>("SettingsButton");
            _buttonList.Add(SettingsButton);

            AboutButton = _root.Q<Button>("AboutButton");
            _buttonList.Add(AboutButton);

            TapToPlay = _root.Q<Label>("TapToPlayText");
            TapToPlay.text = _tapToPlayLanguage.GetText();

            GameVersionText = _root.Q<Label>("GameVersionText");
        }

        private void Start()
        {
            AddSoundToAllButtons();

            PlayButton.clicked += () => OnPlayButtonClicked?.Invoke();
            SettingsButton.clicked += () => OnSettingsButtonClicked?.Invoke();
            AboutButton.clicked += () => OnAboutButtonClicked?.Invoke();
        }

        private void AddSoundToAllButtons()
        {
            foreach (Button button in _buttonList)
                button.clicked += () => PlaySound(SoundType.ButtonClick);
        }

        private void PlaySound(SoundType soundType)
        {
            GeneralSoundManager.Instance.PlaySound(soundType);
        }
    }
}
