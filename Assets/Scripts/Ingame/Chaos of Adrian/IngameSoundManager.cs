using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
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
    public class IngameSoundManager : Singleton<IngameSoundManager>
    {
        [SerializeField]
        private MixinDictionary<SoundType, AudioTrackSetupSOList> _soundList;

        public MixinDictionary<SoundType, AudioTrackSetupSOList> SoundList { get => _soundList; }

        private void Start()
        {
            //EnvironmentManager.OnGameStarted += () => PlaySound(SoundType.StartVoice);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerDeathEvent += () => PlaySound(SoundType.Die);
            EnvironmentManager.Instance.PlayerOperator.OnPlayerTakeDamageEvent += () => PlaySound(SoundType.TakeDamage);
            EnvironmentManager.OnBiomeChanged += (biome) => PlaySound(SoundType.Teleport);
            InputManager.OnPlayerJump += () => PlaySound(SoundType.Jump);
            InputManager.OnPlayerAttack += () => PlaySound(SoundType.Attack);
            InputManager.Instance.InputControls.Ingame.Descend.started += _ => PlaySound(SoundType.Descend);
            CollectableOperator.OnCollected += PlayCollectableSound;
            ObstacleOperator.OnKilled += _ => PlaySound(SoundType.ObstacleKill);
            EnvironmentManager.Instance.PlayerOperator.MeleeSlash.OnBounceObstacleEvent +=
                () => PlaySound(SoundType.PlatformBounce);
            EnvironmentManager.OnGameStarted += () => PlaySound(SoundType.GameStarted);
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

        public void PlaySound(SoundType soundType)
        {
            _soundList[soundType].PlaySound();
        }

        private void PlayCollectableSound(CollectableOperator collectable)
        {
            if (collectable.HealthIncrease >= 1)
                PlaySound(SoundType.HeartRefill);
            else if (collectable.ScoreIncrease >= 1)
                PlaySound(SoundType.Collect);
        }
    }
}
