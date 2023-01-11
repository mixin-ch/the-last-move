using Mixin.Utils;
using Mixin.Utils.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    /// <summary>
    /// Manages the sound.
    /// </summary>
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        private MixinDictionary<SoundType, AudioTrackSetupSO> _soundList;

        public MixinDictionary<SoundType, AudioTrackSetupSO> SoundList { get => _soundList; set => _soundList = value; }

        private void OnEnable()
        {
            EnvironmentManager.OnGameStarted += () => PlaySound(SoundType.GameStarted);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent += () => PlaySound(SoundType.Die);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerTakeDamageEvent += () => PlaySound(SoundType.TakeDamage);
            InputManager.Instance.Input.Ingame.Jump.started += (context) => PlaySound(SoundType.Jump);
            InputManager.Instance.Input.Ingame.Attack.started += (context) => PlaySound(SoundType.Attack);
            InputManager.Instance.Input.Ingame.Descend.started += (context) => PlaySound(SoundType.Descend);
        }

        private void OnDisable()
        {
            EnvironmentManager.OnGameStarted -= () => PlaySound(SoundType.GameStarted);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent -= () => PlaySound(SoundType.Die);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerTakeDamageEvent -= () => PlaySound(SoundType.TakeDamage);
            InputManager.Instance.Input.Ingame.Jump.started -= (context) => PlaySound(SoundType.Jump);
            InputManager.Instance.Input.Ingame.Attack.started -= (context) => PlaySound(SoundType.Attack);
            InputManager.Instance.Input.Ingame.Descend.started -= (context) => PlaySound(SoundType.Descend);
        }

        private void PlaySound(SoundType soundType)
        {
            AudioTrackSetupSO sound = GetSoundTrackFromEnum(soundType);

            if (sound != null)
                AudioManager.Instance.PlayTrack(sound);
        }

        private AudioTrackSetupSO GetSoundTrackFromEnum(SoundType soundType)
        {
            return _soundList.SaveGet(soundType);
        }
    }
}
