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

            _playlist = AudioManager.Instance.MakePlaylistPlayer(_musicPlaylist.ToAudioPlaylistSetup());
            _playlist.Play();
        }
    }
}
