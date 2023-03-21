using Mixin.Utils.Audio;
using Mixin.Utils;
using UnityEngine;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Ads;
using Mixin.TheLastMove.Player;

namespace Mixin.TheLastMove.Ingame
{
    public class IngameSceneManager : Singleton<IngameSceneManager>
    {
        [SerializeField]
        private AudioPlaylistSetupSO _musicPlaylist;

        private AudioPlaylistPlayer _playlist;

        [SerializeField]
        private RewardedAdManager _rewardedAdManager;

        [SerializeField]
        private InterstitialAdManager _interstitialAdManager;
        public RewardedAdManager RewardedAdManager { get => _rewardedAdManager; }
        public InterstitialAdManager InterstitialAdManager { get => _interstitialAdManager; }

        private void Start()
        {
            _playlist = AudioManager.Instance.MakePlaylistPlayer(_musicPlaylist.ToAudioPlaylistSetup());

            //EnvironmentManager.Instance.StartGame();
        }

        private void OnEnable()
        {
            _rewardedAdManager.AdFinished += RewardedAd_OnUserRewarded;
            PlayerOperator.OnPlayerDeathEvent += PlayerOperator_OnPlayerDeathEvent;
            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
        }

        private void OnDisable()
        {
            _rewardedAdManager.AdFinished -= RewardedAd_OnUserRewarded;
            PlayerOperator.OnPlayerDeathEvent -= PlayerOperator_OnPlayerDeathEvent;
            EnvironmentManager.OnGameStarted -= EnvironmentManager_OnGameStarted;
        }

        private void EnvironmentManager_OnGameStarted()
        {
            _playlist.Play();
        }

        private void PlayerOperator_OnPlayerDeathEvent(PlayerOperator _)
        {
            _playlist.Stop();
        }

        private void RewardedAd_OnUserRewarded()
        {
            EnvironmentManager.Instance.Continue();
        }
    }
}
