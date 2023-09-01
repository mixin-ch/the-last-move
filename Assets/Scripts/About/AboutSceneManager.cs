using UnityEngine;
using Mixin.Utils;
using Mixin.TheLastMove.Sound;
using UnityEngine.SceneManagement;

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

        private AboutUIB _uib;

        private void Awake()
        {
            _uib = AboutUIB.Instance;
        }

        private void OnEnable()
        {
            _uib.ExitButton.clicked += ExitButton_clicked;
            _uib.ProjectWebsiteButton.clicked += () => OpenURL(_projectWebsiteURL);
            _uib.redgreenbirdFilmsLogo.clicked += () => OpenURL(_redgreenbirdFilmsURL);
            _uib.MixinGamesLogo.clicked += () => OpenURL(_mixinGamesURL);
            _uib.PrivacyButton.clicked += () => OpenURL(_privacyURL);
        }
        private void OnDisable()
        {
            _uib.ExitButton.clicked -= ExitButton_clicked;
            _uib.ProjectWebsiteButton.clicked -= () => OpenURL(_projectWebsiteURL);
            _uib.redgreenbirdFilmsLogo.clicked -= () => OpenURL(_redgreenbirdFilmsURL);
            _uib.MixinGamesLogo.clicked -= () => OpenURL(_mixinGamesURL);
            _uib.PrivacyButton.clicked -= () => OpenURL(_privacyURL);
        }

        private void ExitButton_clicked()
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SceneName.About.ToString());
            GeneralSoundManager.Instance.PlaySound(SoundType.ButtonClick);
        }

        private void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
