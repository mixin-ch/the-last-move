using Mixin.TheLastMove.Environment;
using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Player;
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

        protected override void Awake()
        {
            base.Awake();

            EnvironmentManager.OnGameStarted += () => PlaySound(SoundType.GameStarted);
        }

        private void Start()
        {
            //EnvironmentManager.OnGameStarted += () => PlaySound(SoundType.StartVoice);
            PlayerOperator.OnPlayerDeathEvent += (_) => PlaySound(SoundType.Die);
            PlayerOperator.OnPlayerTakeDamageEvent += (_) => PlaySound(SoundType.TakeDamage);
            PlayerOperator.OnPlayerLanded += (_) => PlaySound(SoundType.Land);
            EnvironmentManager.OnBiomeChanged += (biome) => PlaySound(SoundType.Teleport);
            InputManager.OnPlayerJump += () => PlaySound(SoundType.Jump);
            InputManager.OnPlayerAttack += () => PlaySound(SoundType.Attack);
            InputManager.Instance.InputControls.Ingame.Descend.started += _ => PlaySound(SoundType.Descend);
            CollectableOperator.OnCollected += PlayCollectableSound;
            ObstacleOperator.OnKilled += _ => PlaySound(SoundType.ObstacleKill);
            EnvironmentManager.Instance.PlayerOperator.MeleeSlash.OnBounceObstacleEvent +=
                () => PlaySound(SoundType.PlatformBounce);
        }

        private void OnDisable()
        {
            PlayerOperator.OnPlayerDeathEvent -= (_) => PlaySound(SoundType.Die);
            PlayerOperator.OnPlayerTakeDamageEvent -= (_) => PlaySound(SoundType.TakeDamage);
            PlayerOperator.OnPlayerLanded -= (_) => PlaySound(SoundType.Land);
            EnvironmentManager.OnBiomeChanged -= (biome) => PlaySound(SoundType.Teleport);
            InputManager.OnPlayerJump -= () => PlaySound(SoundType.Jump);
            InputManager.OnPlayerAttack -= () => PlaySound(SoundType.Attack);
            InputManager.Instance.InputControls.Ingame.Descend.started -= _ => PlaySound(SoundType.Descend);
            CollectableOperator.OnCollected -= PlayCollectableSound;
            ObstacleOperator.OnKilled -= _ => PlaySound(SoundType.ObstacleKill);
            EnvironmentManager.Instance.PlayerOperator.MeleeSlash.OnBounceObstacleEvent -=
                () => PlaySound(SoundType.PlatformBounce);
        }

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
