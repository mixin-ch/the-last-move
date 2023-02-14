using Mixin.Utils.Audio;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Ads;

namespace Mixin.TheLastMove.Ingame
{
    public class IngameSceneManager : Singleton<IngameSceneManager>
    {
        [SerializeField]
        private AudioPlaylistSetupSO _musicPlaylist;

        private AudioPlaylistPlayer _playlist;

        private RewardedAd _rewardedAd = new RewardedAd();

        public RewardedAd RewardedAd { get => _rewardedAd;  }

        protected override void Awake()
        {
            base.Awake();

            _ = _rewardedAd.InitServices();
        }

        private void Start()
        {
            _playlist = AudioManager.Instance.MakePlaylistPlayer(_musicPlaylist.ToAudioPlaylistSetup());

            //EnvironmentManager.Instance.StartGame();
        }

        private void OnEnable()
        {
            _rewardedAd.OnUserRewarded += RewardedAd_OnUserRewarded;
            EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent += PlayerOperator_OnPlayerDeathEvent;
            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
        }

        private void EnvironmentManager_OnGameStarted()
        {
            _playlist.Play();
        }

        private void PlayerOperator_OnPlayerDeathEvent()
        {
            _playlist.Stop();
        }

        private void OnDisable()
        {
            _rewardedAd.OnUserRewarded -= RewardedAd_OnUserRewarded;
        }

        public void ShowRespawnAd()
        {
            _rewardedAd.ShowAd();
        }

        private void RewardedAd_OnUserRewarded()
        {
            // Continue the game
        }
    }
}