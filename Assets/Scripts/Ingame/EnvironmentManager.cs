using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Ingame;
using Mixin.TheLastMove.Player;
using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private PlayerOperator _playerOperator;
        [SerializeField]
        private CollectableSpawner _collectableSpawner;

        [SerializeField]
        private List<BiomeSO> _biomeList;

        private const float _hecticStart = 1;
        private const float _hecticGain = 0.05f;
        private const float _maxHectic = 5f;
        private const float _velocityScale = 3f;
        private const float _biomeDuration = 10f;

        private bool _started;
        private bool _paused;
        private float _hectic;
        private float _distance;
        private BiomeSO _currentBiome;
        private float _biomeTime;

        public PlayerOperator PlayerOperator { get => _playerOperator; }

        public float Velocity => _hectic * _velocityScale;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }
        public bool IsGameRunning { get => _started && !_paused; }
        public float Hectic { get => _hectic; }
        public float Distance { get => _distance; }
        public BiomeSO CurrentBiome { get => _currentBiome; }

        public static event Action OnGameStarted;

        private void OnEnable()
        {
            IngameOverlayUIB.OnPauseButtonClicked += PauseClicked;
            IngamePauseUIB.OnResumeButtonClicked += UnpauseClicked;
            InputManager.OnJumpClicked += JumpClicked;
            InputManager.OnAttackClicked += AttackClicked;
            _playerOperator.OnPlayerDeathEvent += PauseClicked;
            IngameSceneManager.Instance.RewardedAd.OnUserRewarded += RewardedAd_OnUserRewarded;
        }

        private void OnDisable()
        {
            IngameOverlayUIB.OnPauseButtonClicked -= PauseClicked;
            _playerOperator.OnPlayerDeathEvent -= PauseClicked;
            IngameSceneManager.Instance.RewardedAd.OnUserRewarded -= RewardedAd_OnUserRewarded;
        }

        public void StartGame()
        {
            Clear();

            _started = true;
            OnGameStarted?.Invoke();
        }

        private void Clear()
        {
            _started = false;
            _paused = false;

            MapManager.Instance.Clear();

            _playerOperator.ResetState();

            _hectic = _hecticStart;
            _distance = 0;

            _currentBiome = _biomeList.PickRandom();
            _biomeTime = _biomeDuration;
        }

        private void PauseClicked()
        {
            _paused = true;

            if (!_started)
                return;

            _playerOperator.PauseRefresh();
        }

        private void UnpauseClicked()
        {
            _paused = false;

            if (!_started)
                return;

            _playerOperator.PauseRefresh();
        }

        private void JumpClicked()
        {
            if (!_started || _paused)
                return;

            _playerOperator.TryJump();
        }

        private void AttackClicked()
        {
            if (!_started || _paused)
                return;

            _playerOperator.TryAttack();
        }

        private void FixedUpdate()
        {
            if (!_started || _paused)
                return;

            float time = Time.fixedDeltaTime;
            _hectic = (_hectic + _hecticGain * time).UpperBound(_maxHectic);
            float offset = Velocity * time;
            _distance += offset;
            _biomeTime -= time;

            if (_biomeTime <= 0)
            {
                _currentBiome = _biomeList.PickRandom();
                _biomeTime = _biomeDuration;
            }

            MapManager.Instance.Tick(offset);

            _collectableSpawner.MoveCollectablesWithTerrain(offset);

            _playerOperator.Tick(time);
        }

        private void RewardedAd_OnUserRewarded()
        {
            // ContinueGame();
            StartGame();
        }
    }
}
