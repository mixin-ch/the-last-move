using Mixin.TheLastMove.Player;
using Mixin.Utils;
using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private PlayerOperator _playerOperator;

        [SerializeField]
        private BiomeSO[] _biomeList;

        private const float _hecticStart = 1;
        private const float _hecticGain = 0.05f;
        private const float _maxHectic = 5f;
        private const float _velocityScale = 3f;

        private const int _collectablePoints = 10;
        private const int _obstaclePoints = 10;

        [SerializeField]
        private Vector2Int _biomeDurationMinMax = Vector2Int.one;

        private bool _started;
        private bool _paused;
        private float _hectic;
        private float _distance;
        private BiomeSO _currentBiome;
        private float _biomeTime;

        private int _collectablesCollected;
        private int _obstaclesKilled;

        public static int PlayCounter = 0;

        public PlayerOperator PlayerOperator { get => _playerOperator; }

        public float Velocity => _hectic * _velocityScale;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }
        public bool IsGameRunning { get => _started && !_paused; }
        public float Hectic { get => _hectic; }
        public float Distance { get => _distance; }
        public BiomeSO CurrentBiome { get => _currentBiome; }

        public int CollectablesCollected { get => _collectablesCollected; set => _collectablesCollected = value; }
        public int ObstaclesKilled { get => _obstaclesKilled; set => _obstaclesKilled = value; }

        public int Score { get => Distance.FloorToInt() + CollectablesCollected * _collectablePoints + ObstaclesKilled * _obstaclePoints; }

        public static event Action OnGameStarted;
        public static event Action<BiomeSO> OnBiomeChanged;

        private void OnEnable()
        {
            IngameOverlayUIB.OnPauseButtonClicked += PauseClicked;
            IngamePauseUIB.OnResumeButtonClicked += UnpauseClicked;
            _playerOperator.OnPlayerDeathEvent += _playerOperator_OnPlayerDeathEvent;
            PlayerOperator.OnPlayerTakeDamageEvent += SlowPlayerDown;
        }

        private void OnDisable()
        {
            IngameOverlayUIB.OnPauseButtonClicked -= PauseClicked;
            _playerOperator.OnPlayerDeathEvent -= _playerOperator_OnPlayerDeathEvent;
            PlayerOperator.OnPlayerTakeDamageEvent -= SlowPlayerDown;
        }

        protected override void Awake()
        {
            base.Awake();

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
            _playerOperator.StartPlayer();
            _started = true;
            OnGameStarted?.Invoke();
        }

        public void SoftRestartGame()
        {
            SoftClear();

            _playerOperator.SoftStartPlayer();
            _started = true;
            OnGameStarted?.Invoke();
        }

        private void _playerOperator_OnPlayerDeathEvent()
        {
            PauseClicked();
        }

        public void Continue()
        {
            if (!_started) return;

            SoftRestartGame();
        }

        private void SoftClear()
        {
            _started = false;
            _paused = false;

            MapManager.Instance.Clear();

            _currentBiome = _biomeList.PickRandom();

            // Get random biome duration
            _biomeTime = GetRandomBiomeDuration();
        }

        private void Clear()
        {
            _started = false;
            _paused = false;

            MapManager.Instance.Clear();

            _hectic = _hecticStart;
            _distance = 0;

            _currentBiome = _biomeList.PickRandom();

            // Get random biome duration
            _biomeTime = GetRandomBiomeDuration();

            _collectablesCollected = 0;
            _obstaclesKilled = 0;
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

        public void SlowPlayerDown()
        {
            _hectic *= .7f;
        }
    }
}
