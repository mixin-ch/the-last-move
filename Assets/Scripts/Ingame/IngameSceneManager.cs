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

        protected override void Awake()
        {
            base.Awake();

            _ = _rewardedAd.InitServices();
        }

        private void Start()
        {
            EnvironmentManager.Instance.StartGame();

            _playlist = AudioManager.Instance.MakePlaylistPlayer(_musicPlaylist.ToAudioPlaylistSetup());
            _playlist.Play();
        }

        private void OnEnable()
        {
            _rewardedAd.OnUserRewarded += RewardedAd_OnUserRewarded;
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
