using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngameUIB : UIBuildCollector<IngameUIB>
    {
        private Button PauseButton;

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

            PauseButton = _root.Q<Button>("PauseButton");
            _healthContainer = _root.Q<VisualElement>("HealthContainer");
            ScoreText = _root.Q<Label>("ScoreText");
            KillText = _root.Q<Label>("KillText");
            CurrencyText = _root.Q<Label>("CurrencyText");
        }

        private void Start()
        {
            ResetValues();

            PauseButton.clicked += () => OnPauseButtonClicked?.Invoke();
        }

        private void ResetValues()
        {
            ScoreText.text = "0";
            KillText.text = "0";
            CurrencyText.text = "0";
        }

        [Obsolete]
        public void Init() { }

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
