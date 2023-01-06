using Mixin.Utils;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngameOverlayUIB : UIBuildManager<IngameOverlayUIB>
    {
        private VisualElement _body;
        private Button _pauseButton;

        private VisualElement _healthContainer;
        [SerializeField]
        private VisualTreeAsset _heartTemplate;
        public Label ScoreText { get; set; }
        public Label KillText { get; set; }
        public Label CurrencyText { get; set; }

        public static event Action OnPauseButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            _body = _root.Q<VisualElement>("OverlayBody");

            _pauseButton = _body.Q<Button>("PauseButton");
            _healthContainer = _body.Q<VisualElement>("HealthContainer");
            ScoreText = _body.Q<Label>("ScoreText");
            KillText = _body.Q<Label>("KillText");
            CurrencyText = _body.Q<Label>("CurrencyText");

            _healthContainer.Clear();
        }

        public void Show(bool show)
        {
            _body.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void Start()
        {
            ResetValues();
        }

        private void OnEnable()
        {
            _pauseButton.clicked += () => OnPauseButtonClicked?.Invoke();
        }

        private void OnDisable()
        {
            _pauseButton.clicked -= () => OnPauseButtonClicked?.Invoke();
        }

        public void ResetValues()
        {
            ScoreText.text = "0";
            KillText.text = "0";
            CurrencyText.text = "0";
        }

        public void AddHeart()
        {
            TemplateContainer heart = _heartTemplate.CloneTree();
            _healthContainer.Add(heart);
        }

        public void RemoveHeart()
        {
            _healthContainer.RemoveAt(0);
        }
    }
}
