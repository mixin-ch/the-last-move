using UnityEngine;
using UnityEngine.Advertisements;
using System;

namespace Mixin.TheLastMove.Ads
{
    public class RewardedAdManager : MonoBehaviour, IUnityAdsListener
    {
        // Set your game ID and ad placement ID in the Unity Editor

#if UNITY_IOS
        private string _gameId = "5116920";
        private string _adPlacementId = "Rewarded_iOS";
#else
        private string _gameId = "5116921";
        private string _adPlacementId = "Rewarded_Android";
#endif

        private bool _adIsReady;

        // Define events for ad events
        public event Action AdStarted;
        public event Action AdFinished;
        public event Action AdSkipped;
        public event Action AdFailed;

        private void Start()
        {
            // Initialize Unity Ads
            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId, true);
        }

        public void ShowAd()
        {
            // Check if an ad is ready to show
            if (_adIsReady)
            {
                // Show the ad
                Advertisement.Show(_adPlacementId);
            }
            else
            {
                Debug.LogWarning("Show Ad was called before ad was ready.");
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
            // Set _adIsReady to true when an ad is ready to show
            if (placementId == _adPlacementId)
            {
                _adIsReady = true;
                Debug.Log("Ad is ready");
            }
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Fire an event when the ad finishes playing
            if (placementId == _adPlacementId)
            {
                switch (showResult)
                {
                    case ShowResult.Finished:
                        // Ad finished playing, reward the player
                        AdFinished?.Invoke();
                        break;
                    case ShowResult.Skipped:
                        // Ad was skipped by the player
                        AdSkipped?.Invoke();
                        break;
                    case ShowResult.Failed:
                        // Ad failed to play, show error message
                        AdFailed?.Invoke();
                        break;
                }
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            // Fire an event when the ad starts playing
            if (placementId == _adPlacementId)
            {
                AdStarted?.Invoke();
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            // Show error message if Unity Ads encounters an error
            Debug.LogError("Unity Ads error: " + message);
        }

        private void OnDestroy()
        {
            // Remove the listener when the object is destroyed
            Advertisement.RemoveListener(this);
        }
    }
}