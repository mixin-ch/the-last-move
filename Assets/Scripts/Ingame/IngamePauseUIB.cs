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
        private Button _quitButton;

        public static event Action OnResumeButtonClicked;
        public static event Action OnQuitButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            _body = _root.Q<VisualElement>("PauseBody");

            _resumeButton = _body.Q<Button>("ResumeButton");
            _quitButton = _body.Q<Button>("QuitButton");
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
