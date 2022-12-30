using System;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngameUIB : UIBuildCollector<IngameUIB>
    {
        public Button PauseButton { get; set; }
        public VisualElement HealthContainer { get; set; }

        private VisualTreeAsset _heartTemplate;

        public Label ScoreText { get; set; }
        public Label KillText { get; set; }
        public Label CurrencyText { get; set; }

        public static event Action OnPauseButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            PauseButton = _root.Q<Button>("PauseButton");
            HealthContainer = _root.Q<VisualElement>("HealthContainer");
            ScoreText = _root.Q<Label>("ScoreText");
            KillText = _root.Q<Label>("KillText");
            CurrencyText = _root.Q<Label>("CurrencyText");
        }

        public void Init()
        {
            ScoreText.text = "0";
            KillText.text = "0";
            CurrencyText.text = "0";

            PauseButton.clicked += () => OnPauseButtonClicked?.Invoke();
        }

        public void AddHeart()
        {
            TemplateContainer heart = _heartTemplate.CloneTree();
            HealthContainer.Add(heart);
        }

        public void RemoveHeart()
        {
            HealthContainer.RemoveAt(0);
        }
    }
}
