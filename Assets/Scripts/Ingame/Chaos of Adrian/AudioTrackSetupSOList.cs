using Mixin.Utils;
using Mixin.Utils.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Sound
{
    [Serializable]
    public class AudioTrackSetupSOList
    {
        [SerializeField]
        private bool _playRandom = true;

        [SerializeField]
        private List<AudioTrackSetupSO> _soundList;

        public List<AudioTrackSetupSO> SoundList { get => _soundList; }

        public void PlaySound()
        {
            if (_soundList == null)
                return;

            AudioTrackSetupSO sound;

            if (_playRandom)
                sound = GetRandomSound();
            else
                sound = GetFirstSound();

            // Play the sound
            if (sound != null)
                AudioManager.Instance.PlayTrack(sound);
        }

        public AudioTrackSetupSO GetRandomSound()
        {
            if (_soundList == null)
                return null;

            return _soundList.PickRandom();
        }

        public AudioTrackSetupSO GetFirstSound()
        {
            if (_soundList == null)
                return null;

            return _soundList[0];
        }
    }
}
