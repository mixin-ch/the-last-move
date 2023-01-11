using Mixin.Utils.Audio;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mixin.TheLastMove.Environment;

namespace Mixin.TheLastMove.Ingame
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
