using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Mediation;
using Mixin.Utils;
using System.Threading.Tasks;

namespace Mixin.TheLastMove.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField]
        private string _gameID_android; //optional, we will autofetch the gameID if the project is linked in the dashboard
        [SerializeField]
        private string _gameID_ios;//optional, we will autofetch the gameID if the project is linked in the dashboard
        IRewardedAd _ad;

        public static event Action<ShowErrorEventArgs> OnUnityAdsShowFailure;
        public static event Action<RewardEventArgs> OnUserRewarded;

        private void OnEnable()
        {
            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                InitializationOptions opt = new InitializationOptions();

                string gameID;
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        gameID = _gameID_android;
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        gameID = _gameID_ios;
                        break;
                    default:
                        gameID = _gameID_android;
                        $"The current platform {Application.platform} is not supported for ads".LogWarning();
                        break;
                }
                opt.SetGameId(gameID);

                await UnityServices.InitializeAsync(opt);
                OnInitializationComplete();
            }
            catch (Exception e)
            {
                OnInitializationFailed(e);
            }
        }

        private async void LoadRewardedAd()
        {
            string rewardedAdUnitId = AddPlatformString("Rewarded");

            IRewardedAd rewardedAd;

            rewardedAd = MediationService.Instance.CreateRewardedAd(rewardedAdUnitId);
            _ad = rewardedAd;
            try
            {
                rewardedAd.OnLoaded += OnUnityAdsAdLoaded;
                rewardedAd.OnFailedLoad += OnUnityAdsFailedToLoad;
                rewardedAd.OnShowed += OnUnityAdsShowStart;
                rewardedAd.OnFailedShow += OnUnityAdsShowFailureHandler;
                rewardedAd.OnClicked += OnUnityAdsShowClick;
                rewardedAd.OnClosed += OnUnityAdsClosed;
                rewardedAd.OnUserRewarded += OnUserRewardedHandler;
                await rewardedAd.LoadAsync();
            }
            catch (LoadFailedException) { }
        }

        //this would likely be hooked to a UI button or a game event
        public async Task ShowRewardedAdAsync()
        {
            // Ensure the ad has loaded, then show it
            if (_ad.AdState == AdState.Loaded)
            {
                try
                {
                    await _ad.ShowAsync();
                }
                catch (Exception e)
                {
                    //handle failure here
                }
            }
        }

        private void OnInitializationComplete()
        {
            // We recommend to load right after initialization according to docs
            LoadRewardedAd();
            Debug.Log("Init Success");
        }

        private void OnInitializationFailed(Exception e)
        {
            SdkInitializationError initializationError = SdkInitializationError.Unknown;
            if (e is InitializeFailedException initializeFailedException)
            {
                initializationError = initializeFailedException.initializationError;
            }

            Debug.Log($"{initializationError}:{e.Message}");
        }

        private void OnUnityAdsAdLoaded(object sender, EventArgs e)
        {
            Debug.Log($"Load Success");
        }

        private void OnUnityAdsFailedToLoad(object sender, LoadErrorEventArgs e)
        {
            Debug.Log($"{e.Error}:{e.Message}");
        }

        private void OnUnityAdsShowFailureHandler(object sender, ShowErrorEventArgs args)
        {
            Debug.Log($"Ad failed to show: {args.Error}");
            OnUnityAdsShowFailure?.Invoke(args);
        }

        private void OnUnityAdsShowStart(object sender, EventArgs args)
        {
            Debug.Log("Ad shown successfully.");
        }

        public void OnUnityAdsShowClick(object sender, EventArgs e)
        {
            Debug.Log("Ad show clicked");
        }

        private void OnUnityAdsClosed(object sender, EventArgs e)
        {
            Debug.Log("Ad Closed");
        }
        private void OnUserRewardedHandler(object sender, RewardEventArgs e)
        {
            Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");
            OnUserRewarded?.Invoke(e);
        }

        private static string AddPlatformString(string text)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    text += " Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    text += " iOS";
                    break;
                default:
                    text += " Android";
                    $"The current platform {Application.platform} is not supported for ads".LogWarning();
                    break;
            }

            return text;
        }
    }
}
