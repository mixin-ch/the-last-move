using System;
using UnityEngine;
using Mixin.Utils;

namespace Mixin.TheLastMove.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        protected override void Awake()
        {
            base.Awake();

#if UNITY_ANDROID
            string appKey = "182f7a4cd";
#elif UNITY_IPHONE
        string appKey = "";
#else
        string appKey = "unexpected_platform";
#endif

            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(appKey);
        }

        void OnEnable()
        {
            //Add Init Event
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
/*
            //Add Rewarded Video Events
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;*/
        }

        private void RewardedVideoAdShowFailedEvent(IronSourceError obj)
        {
            throw new NotImplementedException();
        }

        private void RewardedVideoAdRewardedEvent(IronSourcePlacement obj)
        {
            throw new NotImplementedException();
        }

        private void SdkInitializationCompletedEvent()
        {
            throw new NotImplementedException();
        }
    }
}