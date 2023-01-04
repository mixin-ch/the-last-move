using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;

namespace Mixin.TheLastMove.About
{
    public class AboutSceneManager : MonoBehaviour
    {
        private void OnEnable()
        {
            AboutUIB.Instance.ExitButton.clicked += ExitButton_clicked;
        }

        private void ExitButton_clicked()
        {
            SceneManager.Instance.LoadScene(SceneName.MainMenu.ToString());
        }
    }
}
