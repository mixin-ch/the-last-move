using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mixin.Utils;
using UnityEngine.Audio;

namespace Mixin.TheLastMove
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        [SerializeField]
        private AudioMixer _audioMixer;

        public AudioMixer AudioMixer { get => _audioMixer; }

        private void Start()
        {
            // Load User Game Data

            // Load User Settings
            Application.targetFrameRate = 60;

            /*int musicVolume = 100;
            SetVolume(_audioMixer, musicVolume);

            int soundVolume = 100;
            SetVolume(_audioMixer, soundVolume);*/
        }
    }
}
