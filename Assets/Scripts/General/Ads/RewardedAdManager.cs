using UnityEngine;
using UnityEngine.Advertisements;
using System;

namespace Mixin.TheLastMove.Ads
{
    public class RewardedAdManager : MonoBehaviour,
        IUnityAdsInitializationListener,
        IUnityAdsLoadListener,
        IUnityAdsShowListener
    {
        // Set your game ID and ad placement ID in the Unity Editor

#if UNITY_IOS
        private string _gameId = "5116920";
        private string _adPlacementId = "Rewarded_iOS";
#else
        private string _gameId = "5116921";
        private string _adPlacementId = "Rewarded_Android";
#endif

        private bool _testMode = true;

        // Define events for ad events
        public static event Action AdStarted;
        public static event Action AdFinished;
        public static event Action AdSkipped;
        public static event Action AdFailed;

        private void Start()
        {
            // Initialize Unity Ads
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void ShowAd()
        {
            // Check if an ad is ready to show
            if (!Advertisement.isInitialized)
            {
                Debug.LogWarning("Show Ad was called before ad was ready.");
                return;
            }

            // Show the ad
            Advertisement.Show(_adPlacementId);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            // Ad failed to play, show error message
            AdFailed?.Invoke();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            AdStarted?.Invoke();
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            // Ad finished playing, reward the player
            AdFinished?.Invoke();
        }

        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogWarning(error + message);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"Ad {placementId} loaded");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new NotImplementedException();
        }
    }
}