using Mixin.TheLastMove.Environment;
using Mixin.Utils;
using Mixin.Utils.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Sound
{
    /// <summary>
    /// Manages the sound.
    /// </summary>
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        private MixinDictionary<SoundType, AudioTrackSetupSO> _soundList;

        [SerializeField]
        private List<AudioTrackSetupSO> _startVoices;

        public MixinDictionary<SoundType, AudioTrackSetupSO> SoundList { get => _soundList; }
        public List<AudioTrackSetupSO> StartVoices { get => _startVoices; }

        private void Start()
        {
            EnvironmentManager.OnGameStarted += () => PlaySound(GetRandomStartVoice());
            EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent += () => PlaySound(SoundType.Die);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerTakeDamageEvent += () => PlaySound(SoundType.TakeDamage);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerAttackEvent += () => PlaySound(SoundType.Attack);
            InputManager.Instance.Input.Ingame.Jump.started += (context) => PlaySound(SoundType.Jump);
            //InputManager.Instance.Input.Ingame.Attack.started += (context) => PlaySound(SoundType.Attack);
            InputManager.Instance.Input.Ingame.Descend.started += (context) => PlaySound(SoundType.Descend);
        }

        /* private void OnDisable()
         {
             EnvironmentManager.OnGameStarted -= () => PlaySound(SoundType.GameStarted);
             EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent -= () => PlaySound(SoundType.Die);
             EnvironmentManager.Instance.PlayerOperator.OnPlayerTakeDamageEvent -= () => PlaySound(SoundType.TakeDamage);
             InputManager.Instance.Input.Ingame.Jump.started -= (context) => PlaySound(SoundType.Jump);
             InputManager.Instance.Input.Ingame.Attack.started -= (context) => PlaySound(SoundType.Attack);
             InputManager.Instance.Input.Ingame.Descend.started -= (context) => PlaySound(SoundType.Descend);
         }*/

        private void PlaySound(SoundType soundType)
        {
            AudioTrackSetupSO sound = GetSoundTrackFromEnum(soundType);

            if (sound != null)
                AudioManager.Instance.PlayTrack(sound);
        }

        private void PlaySound(AudioTrackSetupSO audioTrackSetupSO)
        {
            if (audioTrackSetupSO == null)
                return;

            AudioManager.Instance.PlayTrack(audioTrackSetupSO);
        }

        private AudioTrackSetupSO GetRandomStartVoice()
        {
            if (_startVoices == null || _startVoices.Count < 1)
                return null;

            return _startVoices.PickRandom(); ;
        }

        private AudioTrackSetupSO GetSoundTrackFromEnum(SoundType soundType)
        {
            return _soundList.SaveGet(soundType);
        }
    }
}
