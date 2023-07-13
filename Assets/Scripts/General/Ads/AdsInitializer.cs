using UnityEngine;
using UnityEngine.Advertisements;

namespace Mixin.TheLastMove.Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField]
        private RewardedAdsButton _rewardedAdsButton;

        [SerializeField]
        private InterstitialAd _interstitialAd;

        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        [SerializeField] bool _testMode = true;
        private string _gameId;

        void Awake()
        {
            InitializeAds();
        }

        private void Start()
        {
            if (Advertisement.isInitialized)
                LoadAds();
        }

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");

            LoadAds();
        }

        public void LoadAds()
        {
            _rewardedAdsButton.LoadAd();
            _interstitialAd.LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}