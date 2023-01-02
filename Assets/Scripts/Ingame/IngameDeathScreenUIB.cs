using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngameDeathScreenUIB : UIBuildManager<IngameDeathScreenUIB>
    {
        private VisualElement _body;
        private Button _respawnButton;
        private Button _restartButton;
        private Button _quitButton;
        public Label ScoreText { get; set; }
        public Label KillText { get; set; }
        public Label CurrencyText { get; set; }

        public static event Action OnRespawnButtonClicked;
        public static event Action OnRestartButtonClicked;
        public static event Action OnQuitButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            _body = _root.Q<VisualElement>("DeathScreenBody");

            _respawnButton = _body.Q<Button>("RespawnButton");
            _restartButton = _body.Q<Button>("RestartButton");
            _quitButton = _body.Q<Button>("QuitButton");
            ScoreText = _body.Q<Label>("ScoreText");
            KillText = _body.Q<Label>("KillText");
            CurrencyText = _body.Q<Label>("CurrencyText");
        }

        public void Show(bool show)
        {
            _body.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnEnable()
        {
            _respawnButton.clicked += () => OnRespawnButtonClicked?.Invoke();
            _restartButton.clicked += () => OnRestartButtonClicked?.Invoke();
            _quitButton.clicked += () => OnQuitButtonClicked?.Invoke();
        }

        private void OnDisable()
        {
            _respawnButton.clicked -= () => OnRespawnButtonClicked?.Invoke();
            _restartButton.clicked -= () => OnRestartButtonClicked?.Invoke();
            _quitButton.clicked -= () => OnQuitButtonClicked?.Invoke();
        }
    }
}
