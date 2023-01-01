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

        public static event Action OnPlayButtonClicked;
        public static event Action OnSettingsButtonClicked;
        public static event Action OnAboutButtonClicked;

        protected override void Awake()
        {
            base.Awake();

            PlayButton = _root.Q<Button>("PlayButton");
            SettingsButton = _root.Q<Button>("SettingsButton");
            AboutButton = _root.Q<Button>("AboutButton");
        }

        private void Start()
        {
            PlayButton.clicked += () => OnPlayButtonClicked?.Invoke();
            SettingsButton.clicked += () => OnSettingsButtonClicked?.Invoke();
            AboutButton.clicked += () => OnAboutButtonClicked?.Invoke();
        }
    }
}
