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
        private BiomeSO[] _biomeList;

        /*/// <summary>
        /// A list of all blockoperators.
        /// Inited at Awake.
        /// </summary>
        private BlockOperator[] _blockOperatorList;*/

        private const float _hecticStart = 1;
        private const float _hecticGain = 0.05f;
        private const float _maxHectic = 5f;
        private const float _velocityScale = 3f;

        [SerializeField]
        private Vector2Int _biomeDurationMinMax = Vector2Int.one;

        private bool _started;
        private bool _paused;
        private float _hectic;
        private float _distance;
        private BiomeSO _currentBiome;
        private float _biomeTime;

        public static int PlayCounter = 0;

        public PlayerOperator PlayerOperator { get => _playerOperator; }

        public float Velocity => _hectic * _velocityScale;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }
        public bool IsGameRunning { get => _started && !_paused; }
        public float Hectic { get => _hectic; }
        public float Distance { get => _distance; }
        public BiomeSO CurrentBiome { get => _currentBiome; }

        public static event Action OnGameStarted;
        public static event Action<BiomeSO> OnBiomeChanged;

        private void OnEnable()
        {
            IngameOverlayUIB.OnPauseButtonClicked += PauseClicked;
            IngamePauseUIB.OnResumeButtonClicked += UnpauseClicked;
            _playerOperator.OnPlayerDeathEvent += _playerOperator_OnPlayerDeathEvent;
        }

        private void OnDisable()
        {
            IngameOverlayUIB.OnPauseButtonClicked -= PauseClicked;
            _playerOperator.OnPlayerDeathEvent -= _playerOperator_OnPlayerDeathEvent;
        }

        protected override void Awake()
        {
            base.Awake();

            /*for (int i = 0; i < _biomeList.Length; i++)
            {
                // Add BlockOperator to list. This reduces requests.
                _blockOperatorList[i] = _biomeList[i].Prefab.GetComponent<BlockOperator>();
            }*/

            PlayCounter = 0;
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            Clear();

            BackgroundManager.Instance.Init();

            PlayCounter++;
            _started = true;
            OnGameStarted?.Invoke();
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            PauseClicked();
            _started = false;
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

            // Get random biome duration
            _biomeTime = GetRandomBiomeDuration();
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
                BiomeSO newBiome = _biomeList.PickRandom();
                _biomeTime = GetRandomBiomeDuration();

                if (newBiome == _currentBiome)
                    return;

                _currentBiome = newBiome;

                OnBiomeChanged?.Invoke(_currentBiome);
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

        private float GetRandomBiomeDuration()
        {
            return new System.Random().Range(_biomeDurationMinMax.x, _biomeDurationMinMax.y);
        }
    }
}
