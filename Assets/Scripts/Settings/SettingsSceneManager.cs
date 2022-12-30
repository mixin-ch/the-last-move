using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using System;

namespace Mixin.TheLastMove
{
    public class SettingsSceneManager : MonoBehaviour
    {
        private void Start()
        {

        }

        private void OnEnable()
        {
            SettingsUIB.OnExitButtonClicked += OnExitButtonClicked;
        }

        private void OnDisable()
        {
            SettingsUIB.OnExitButtonClicked -= OnExitButtonClicked;
        }

        public void LoadSettings()
        {

        }

        private void SaveSettings()
        {

        }

        private void OnExitButtonClicked()
        {
            // Change Scene
        }
    }
}
