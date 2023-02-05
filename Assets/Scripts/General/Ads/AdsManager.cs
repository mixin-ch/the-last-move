using System;
using UnityEngine;
using Mixin.Utils;

namespace Mixin.TheLastMove.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField]
        private string _appKey;

        protected override void Awake()
        {
            base.Awake();

            IronSource.Agent.init(
                _appKey,
                IronSourceAdUnits.REWARDED_VIDEO,
                IronSourceAdUnits.INTERSTITIAL);

            IronSource.Agent.validateIntegration();
        }
    }
}