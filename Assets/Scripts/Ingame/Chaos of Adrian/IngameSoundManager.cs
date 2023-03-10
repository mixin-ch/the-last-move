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

            EnvironmentManager.OnGameStarted += EnvironmentManager_OnGameStarted;
        }

        private void Start()
        {
            PlayerOperator.OnPlayerDeathEvent += PlayerOperator_OnPlayerDeathEvent;
            PlayerOperator.OnPlayerTakeDamageEvent += PlayerOperator_OnPlayerTakeDamageEvent;
            PlayerOperator.OnPlayerLanded += PlayerOperator_OnPlayerLanded;
            EnvironmentManager.OnBiomeChanged += EnvironmentManager_OnBiomeChanged;
            InputManager.OnPlayerJump += InputManager_OnPlayerJump;
            InputManager.OnPlayerAttack += InputManager_OnPlayerAttack;
            InputManager.Instance.InputControls.Ingame.Descend.started += Descend_started;
            CollectableOperator.OnCollected += PlayCollectableSound;
            ObstacleOperator.OnKilled += ObstacleOperator_OnKilled;
            EnvironmentManager.Instance.PlayerOperator.MeleeSlash.OnBounceObstacleEvent +=
                MeleeSlash_OnBounceObstacleEvent;

            // Button Press
            IngameDeathScreenUIB.OnQuitButtonClicked += IngameDeathScreenUIB_OnQuitButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked += IngameDeathScreenUIB_OnRespawnButtonClicked;
            IngameDeathScreenUIB.OnRestartButtonClicked += IngameDeathScreenUIB_OnRestartButtonClicked;
            IngamePauseUIB.OnQuitButtonClicked += IngamePauseUIB_OnQuitButtonClicked;
            IngamePauseUIB.OnResumeButtonClicked += IngamePauseUIB_OnResumeButtonClicked;
        }

        private void OnDisable()
        {
            EnvironmentManager.OnGameStarted -= EnvironmentManager_OnGameStarted;

            PlayerOperator.OnPlayerDeathEvent -= PlayerOperator_OnPlayerDeathEvent;
            PlayerOperator.OnPlayerTakeDamageEvent -= PlayerOperator_OnPlayerTakeDamageEvent;
            PlayerOperator.OnPlayerLanded -= PlayerOperator_OnPlayerLanded;
            EnvironmentManager.OnBiomeChanged -= EnvironmentManager_OnBiomeChanged;
            InputManager.OnPlayerJump -= InputManager_OnPlayerJump;
            InputManager.OnPlayerAttack -= InputManager_OnPlayerAttack;
            InputManager.Instance.InputControls.Ingame.Descend.started -= Descend_started;
            CollectableOperator.OnCollected -= PlayCollectableSound;
            ObstacleOperator.OnKilled -= ObstacleOperator_OnKilled;
            EnvironmentManager.Instance.PlayerOperator.MeleeSlash.OnBounceObstacleEvent -=
                MeleeSlash_OnBounceObstacleEvent;

            // Button Press
            IngameDeathScreenUIB.OnQuitButtonClicked -= IngameDeathScreenUIB_OnQuitButtonClicked;
            IngameDeathScreenUIB.OnRespawnButtonClicked -= IngameDeathScreenUIB_OnRespawnButtonClicked;
            IngameDeathScreenUIB.OnRestartButtonClicked -= IngameDeathScreenUIB_OnRestartButtonClicked;
            IngamePauseUIB.OnQuitButtonClicked -= IngamePauseUIB_OnQuitButtonClicked;
            IngamePauseUIB.OnResumeButtonClicked -= IngamePauseUIB_OnResumeButtonClicked;
        }

        public void PlaySound(SoundType soundType)
        {
            _soundList[soundType].PlaySound();
        }

        private void PlayGeneralSound(SoundType soundType)
        {
            GeneralSoundManager.Instance.PlaySound(soundType);
        }

        private void PlayCollectableSound(CollectableOperator collectable)
        {
            if (collectable.HealthIncrease >= 1)
                PlaySound(SoundType.HeartRefill);
            else if (collectable.ScoreIncrease >= 1)
                PlaySound(SoundType.Collect);
        }

        private void EnvironmentManager_OnGameStarted()
        {
            PlaySound(SoundType.GameStarted);
        }

        private void IngamePauseUIB_OnResumeButtonClicked()
        {
            PlayGeneralSound(SoundType.ButtonClick);
        }

        private void IngamePauseUIB_OnQuitButtonClicked()
        {
            PlayGeneralSound(SoundType.ButtonClick);
        }

        private void IngameDeathScreenUIB_OnRestartButtonClicked()
        {
            PlayGeneralSound(SoundType.ButtonClick);
        }

        private void IngameDeathScreenUIB_OnRespawnButtonClicked()
        {
            PlayGeneralSound(SoundType.ButtonClick);
        }

        private void IngameDeathScreenUIB_OnQuitButtonClicked()
        {
            PlayGeneralSound(SoundType.ButtonClick);
        }

        private void MeleeSlash_OnBounceObstacleEvent()
        {
            PlaySound(SoundType.PlatformBounce);
        }

        private void ObstacleOperator_OnKilled(ObstacleOperator obj)
        {
            PlaySound(SoundType.ObstacleKill);
        }

        private void Descend_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            PlaySound(SoundType.Descend);
        }

        private void InputManager_OnPlayerAttack()
        {
            PlaySound(SoundType.Attack);
        }

        private void InputManager_OnPlayerJump()
        {
            PlaySound(SoundType.Jump);
        }

        private void EnvironmentManager_OnBiomeChanged(BiomeSO obj)
        {
            PlaySound(SoundType.Teleport);
        }

        private void PlayerOperator_OnPlayerLanded(PlayerOperator obj)
        {
            PlaySound(SoundType.Land);
        }

        private void PlayerOperator_OnPlayerTakeDamageEvent(PlayerOperator obj)
        {
            PlaySound(SoundType.TakeDamage);
        }

        private void PlayerOperator_OnPlayerDeathEvent(PlayerOperator obj)
        {
            PlaySound(SoundType.Die);
        }
    }
}
