using Mixin.Utils;
using UnityEngine.UIElements;
using System;

namespace Mixin.TheLastMove.About
{
    public class AboutUIB : UIBuildManager<AboutUIB>
    {
        public Button ExitButton;

        protected override void Awake()
        {
            base.Awake();

            ExitButton = _root.Q<Button>("ExitButton");
        }
    }
}
