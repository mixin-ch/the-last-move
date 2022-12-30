using Mixin.Audio;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class IngameSceneManager : Singleton<IngameSceneManager>
    {
        [SerializeField]
        private AudioPlaylistSetupSO _musicPlaylist;

        private AudioPlaylistPlayer _playlist;

        private void Start()
        {
            EnvironmentManager.Instance.StartGame();
            IngameUIB.Instance.Init();

            _playlist = AudioManager.Instance.MakePlaylist(_musicPlaylist.ToAudioPlaylistSetup());
            _playlist.Play();
        }

        private void OnPauseButtonClicked()
        {
            if (_playlist.Running)
                _playlist.Stop();
            else
                _playlist.Play();
        }

        private void OnEnable()
        {
            IngameUIB.OnPauseButtonClicked += OnPauseButtonClicked;
        }

        private void OnDisable()
        {
            IngameUIB.OnPauseButtonClicked -= OnPauseButtonClicked;
        }
    }
}
