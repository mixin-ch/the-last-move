using Mixin.MultiLanguage;
using Mixin.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class IngamePauseUIB : UIBuildManager<IngamePauseUIB>
    {
        private VisualElement _body;

        private Button _resumeButton;
        [SerializeField]
        private LanguageTextSO _resumeLanguage;

        private Button _quitButton;
        [SerializeField]
        private LanguageTextSO _quitLanguage;

        public static event Action OnResumeButtonClicked;
        public static event Action OnQuitButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            _body = _root.Q<VisualElement>("PauseBody");

            _resumeButton = _body.Q<Button>("ResumeButton");
            _resumeButton.text = _resumeLanguage.GetText();

            _quitButton = _body.Q<Button>("QuitButton");
            _quitButton.text = _quitLanguage.GetText();
        }

        public void Show(bool show)
        {
            _body.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnEnable()
        {
            _resumeButton.clicked += () => OnResumeButtonClicked?.Invoke();
            _quitButton.clicked += () => OnQuitButtonClicked?.Invoke();
        }

        private void OnDisable()
        {
            _resumeButton.clicked -= () => OnResumeButtonClicked?.Invoke();
            _quitButton.clicked -= () => OnQuitButtonClicked?.Invoke();
        }
    }
}
