using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using UnityEngine.Audio;

namespace Mixin.TheLastMove
{
    public class ApplicationManager : ApplicationManagerBase
    {
        [SerializeField]
        private AudioMixer _audioMixer;

        private void Start()
        {
            // Load User Game Data

            // Load User Settings
            SetFramerate(60);

            int musicVolume = 100;
            SetVolume(_audioMixer, musicVolume);

            int soundVolume = 100;
            SetVolume(_audioMixer, soundVolume);

            int quality = 6;
            SetQuality(quality);
        }
    }
}
