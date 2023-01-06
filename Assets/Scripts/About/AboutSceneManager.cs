using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;

namespace Mixin.TheLastMove.About
{
    public class AboutSceneManager : MonoBehaviour
    {
        [SerializeField]
        private string _projectWebsiteURL;

        [SerializeField]
        private string _redgreenbirdFilmsURL;

        [SerializeField]
        private string _mixinGamesURL;

        [SerializeField]
        private string _privacyURL;

        private AboutUIB _uib => AboutUIB.Instance;

        private void OnEnable()
        {
            _uib.ExitButton.clicked += ExitButton_clicked;
            _uib.ProjectWebsiteButton.clicked += () => OpenURL(_projectWebsiteURL);
            _uib.redgreenbirdFilmsLogo.clicked += () => OpenURL(_redgreenbirdFilmsURL);
            _uib.MixinGamesLogo.clicked += () => OpenURL(_mixinGamesURL);
            _uib.PrivacyButton.clicked += () => OpenURL(_privacyURL);
        }

        private void ExitButton_clicked()
        {
            SceneManager.Instance.LoadScene(SceneName.MainMenu.ToString());
        }

        private void OpenURL(string url)
        {
            Application.OpenURL(url);
        }

        private void RedgreenbirdFilmsLogo_clicked()
        {
            throw new System.NotImplementedException();
        }

        private void MixinGamesLogo_clicked()
        {
            throw new System.NotImplementedException();
        }

        private void PrivacyButton_clicked()
        {
            throw new System.NotImplementedException();
        }
    }
}
