using Mixin.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private GameObject _blockContainer;
        [SerializeField]
        private GameObject _obstacleContainer;
        [SerializeField]
        private GameObject _blockPrefab;
        [SerializeField]
        private GameObject _obstaclePrefab;
        [SerializeField]
        private PlayerOperator _playerOperator;

        private const float _blockSize = 1f;
        private const float _insertDistance = 15f;
        private const float _deleteDistance = 15f;
        private const float _maxInsertHeight = 0f;
        private const float _minInsertHeight = -5f;
        private const float _hecticStart = 1;
        private const float _hecticGain = 0.05f;
        private const float _maxHectic = 5f;
        private const float _velocityScale = 3f;

        private MapGenerator _mapGenerator = new MapGenerator();

        private bool _started;
        private bool _paused;
        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private List<ObstacleOperator> _obstacleOperatorList = new List<ObstacleOperator>();
        private float _hectic;
        private float _distance;
        private float _distancePlanned;

        public float Velocity => _hectic * _velocityScale;

        public static float BlockSize => _blockSize;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }
        public float Hectic { get => _hectic; }
        public float Distance { get => _distance; }

        public static event Action OnGameStarted;

        private void OnEnable()
        {
            IngameOverlayUIB.OnPauseButtonClicked += PauseClicked;
            IngamePauseUIB.OnResumeButtonClicked += UnpauseClicked;
            InputManager.OnJumpClicked += JumpClicked;
            _playerOperator.OnPlayerDeathEvent += PauseClicked;
        }

        private void OnDisable()
        {
            IngameOverlayUIB.OnPauseButtonClicked -= PauseClicked;
        }

        public void StartGame()
        {
            Clear();

            OnGameStarted?.Invoke();

            _started = true;
        }

        private void Clear()
        {
            _started = false;
            _paused = false;

            _playerOperator.ResetState();

            _blockContainer.DestroyChildren();
            _blockOperatorList.Clear();

            _obstacleContainer.DestroyChildren();
            _obstacleOperatorList.Clear();

            _mapGenerator = new MapGenerator();

            _hectic = _hecticStart;
            _distance = 0;
            _distancePlanned = 20;
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

        private void FixedUpdate()
        {
            if (!_started || _paused)
                return;

            float time = Time.fixedDeltaTime;
            _hectic = (_hectic + _hecticGain * time).UpperBound(_maxHectic);
            float offset = Velocity * time;
            _distance += offset;

            TickBlocks(offset);
            TickObstacles(offset);
            TickMapGeneration(offset);

            _playerOperator.Tick(time);
        }

        private void TickBlocks(float offset)
        {
            foreach (BlockOperator @operator in _blockOperatorList)
                @operator.Move(Vector2.left * offset);

            for (int i = 0; i < _blockOperatorList.Count; i++)
            {
                BlockOperator @operator = _blockOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _blockOperatorList.Remove(@operator);
                    @operator.Destroy();
                    i--;
                }
            }
        }

        private void TickObstacles(float offset)
        {
            foreach (ObstacleOperator @operator in _obstacleOperatorList)
                @operator.Move(Vector2.left * offset);

            for (int i = 0; i < _obstacleOperatorList.Count; i++)
            {
                ObstacleOperator @operator = _obstacleOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _obstacleOperatorList.Remove(@operator);
                    @operator.Destroy();
                    i--;
                }
            }
        }

        private void TickMapGeneration(float offset)
        {
            _distancePlanned += offset;

            while (_distancePlanned > 0)
            {
                PlaceMapPlan(_mapGenerator.Tick(Hectic / BlockSize), _insertDistance - _distancePlanned);
                _distancePlanned -= BlockSize;
            }
        }

        private void PlaceMapPlan(MapPlan mapPlan, float x)
        {
            foreach (BlockPlan plan in mapPlan.BlockPlanList)
            {
                GameObject gameObject = _blockPrefab.GeneratePrefab(_blockContainer);
                BlockOperator @operator = gameObject.GetComponent<BlockOperator>();
                float y = Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(new Vector2(x, y), _blockSize);
                _blockOperatorList.Add(@operator);
            }

            foreach (ObstaclePlan plan in mapPlan.ObstaclePlanList)
            {
                GameObject gameObject = _obstaclePrefab.GeneratePrefab(_obstacleContainer);
                ObstacleOperator @operator = gameObject.GetComponent<ObstacleOperator>();
                float y = _blockSize + Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(new Vector2(x, y), _blockSize);
                _obstacleOperatorList.Add(@operator);
            }
        }
    }
}
