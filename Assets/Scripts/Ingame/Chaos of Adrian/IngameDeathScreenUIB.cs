using Mixin.MultiLanguage;
using Mixin.Utils;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngameDeathScreenUIB : UIBuildManager<IngameDeathScreenUIB>
    {
        private VisualElement _body;

        private Button _respawnButton;
        [SerializeField]
        private LanguageTextSO _respawnLanguage;

        private Button _restartButton;
        [SerializeField]
        private LanguageTextSO _restartLanguage;

        private Button _quitButton;
        [SerializeField]
        private LanguageTextSO _quitLanguage;
        public Label ScoreText { get; set; }
        public Label HighscoreText { get; set; }
        public Label KillText { get; set; }
        public Label CurrencyText { get; set; }
        public Button RespawnButton { get => _respawnButton; }

        public static event Action OnRespawnButtonClicked;
        public static event Action OnRestartButtonClicked;
        public static event Action OnQuitButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            _body = _root.Q<VisualElement>("DeathScreenBody");

            _respawnButton = _body.Q<Button>("RespawnButton");
            _respawnButton.text = _respawnLanguage.GetText();

            _restartButton = _body.Q<Button>("RestartButton");
            _restartButton.text = _restartLanguage.GetText();

            _quitButton = _body.Q<Button>("QuitButton");
            _quitButton.text = _quitLanguage.GetText();

            ScoreText = _body.Q<Label>("ScoreText");
            HighscoreText = _body.Q<Label>("HighscoreText");
            KillText = _body.Q<Label>("KillText");
            CurrencyText = _body.Q<Label>("CurrencyText");

            _respawnButton.clicked += () => OnRespawnButtonClicked?.Invoke();
            _restartButton.clicked += () => OnRestartButtonClicked?.Invoke();
            _quitButton.clicked += () => OnQuitButtonClicked?.Invoke();
        }

        public void Show(bool show)
        {
            _body.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnDestroy()
        {
            _respawnButton.clicked -= () => OnRespawnButtonClicked?.Invoke();
            _restartButton.clicked -= () => OnRestartButtonClicked?.Invoke();
            _quitButton.clicked -= () => OnQuitButtonClicked?.Invoke();
        }
    }
}
