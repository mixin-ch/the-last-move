using Mixin.Utils;
using UnityEngine.UIElements;
using System;
using Mixin.MultiLanguage;
using UnityEngine;

namespace Mixin.TheLastMove.About
{
    public class AboutUIB : UIBuildManager<AboutUIB>
    {
        [SerializeField]
        private LanguageTextSO _aboutText;

        [SerializeField]
        private LanguageTextSO _websiteText;

        [SerializeField]
        private LanguageTextSO _privacyText;

        public Label PageTitle { get; set; }
        public Button ExitButton { get; set; }
        //public Label AboutText { get; set; }
        public Button ProjectWebsiteButton { get; set; }
        public Button redgreenbirdFilmsLogo { get; set; }
        public Button MixinGamesLogo { get; set; }
        public Button PrivacyButton { get; set; }

        protected override void Awake()
        {
            base.Awake();

            PageTitle = _root.Q<Label>("PageTitle");
            ExitButton = _root.Q<Button>("ExitButton");
            //AboutText = _root.Q<Label>("ExitButton");
            ProjectWebsiteButton = _root.Q<Button>("ProjectWebsiteButton");
            redgreenbirdFilmsLogo = _root.Q<Button>("redgreenbirdFilmsLogo");
            MixinGamesLogo = _root.Q<Button>("MixinGamesLogo");
            PrivacyButton = _root.Q<Button>("PrivacyButton");
        }

        private void Start()
        {
            PageTitle.text = _aboutText.GetText();
            ProjectWebsiteButton.text = _websiteText.GetText();
            PrivacyButton.text = _privacyText.GetText();
        }
    }
}
