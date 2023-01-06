using Mixin.Utils;
using UnityEngine.UIElements;
using System;

namespace Mixin.TheLastMove.About
{
    public class AboutUIB : UIBuildManager<AboutUIB>
    {
        public Button ExitButton { get; set; }
        public Label AboutText { get; set; }
        public Button ProjectWebsiteButton { get; set; }
        public Button redgreenbirdFilmsLogo { get; set; }
        public Button MixinGamesLogo { get; set; }
        public Button PrivacyButton { get; set; }

        protected override void Awake()
        {
            base.Awake();

            ExitButton = _root.Q<Button>("ExitButton");
            AboutText = _root.Q<Label>("ExitButton");
            ProjectWebsiteButton = _root.Q<Button>("ProjectWebsiteButton");
            redgreenbirdFilmsLogo = _root.Q<Button>("redgreenbirdFilmsLogo");
            MixinGamesLogo = _root.Q<Button>("MixinGamesLogo");
            PrivacyButton = _root.Q<Button>("PrivacyButton");
        }
    }
}
