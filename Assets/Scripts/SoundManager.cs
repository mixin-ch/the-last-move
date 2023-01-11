using Mixin.Utils;
using Mixin.Utils.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        private MixinDictionary<SoundType, AudioTrackSetupSO> _audioList;

        public MixinDictionary<SoundType, AudioTrackSetupSO> AudioList { get => _audioList; set => _audioList = value; }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}
