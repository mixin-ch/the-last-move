using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private GameObject _blockContainer;
        [SerializeField]
        private GameObject _playerContainer;
        [SerializeField]
        private GameObject _blockPrefab;
        [SerializeField]
        private GameObject _playerPrefab;

        private const float _blockSize = 1f;
        private const int _blockRows = 3;
        private const float _lowestRowY = -5f;
        private const float _blockInsertDistance = 15f;
        private const float _blockDeleteDistance = 15f;
        private const float _hecticStart = 1;
        private const float _hecticGain = 0.05f;
        private const float _maxHectic = 5f;
        private const float _velocityScale = 3f;

        private BlockMaker _blockMaker = new BlockMaker();

        private bool _started;
        private bool _paused;
        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private PlayerOperator _playerOperator;
        private float _hectic;
        private float _distance;
        private float _distancePlanned;

        private float Velocity => _hectic * _velocityScale;

        public static float BlockSize => _blockSize;
        public static float BlockRows => _blockRows;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }
        public float Hectic { get => _hectic; }
        public float Distance { get => _distance; }

        private void OnEnable()
        {
            IngameOverlayUIB.OnPauseButtonClicked += PauseClicked;
            InputManager.OnJumpClicked += JumpClicked;
        }

        private void OnDisable()
        {
            IngameOverlayUIB.OnPauseButtonClicked -= PauseClicked;
        }

        public void StartGame()
        {
            Clear();

            GameObject player = _playerPrefab.GeneratePrefab(_playerContainer);
            PlayerOperator playerOperator = player.GetComponent<PlayerOperator>();
            playerOperator.Setup();
            _playerOperator = playerOperator;

            _started = true;
        }

        private void Clear()
        {
            _started = false;
            _paused = false;
            _blockContainer.DestroyChildren();
            _playerContainer.DestroyChildren();
            _blockOperatorList.Clear();
            _playerOperator = null;
            _blockMaker.Initialize();
            _hectic = _hecticStart;
            _distance = 0;
            _distancePlanned = _blockInsertDistance + _blockDeleteDistance;
        }

        private void PauseClicked()
        {
            if (!_started)
                return;

            _paused = !_paused;
            _playerOperator.Pause();
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
            TickBlocks(time);
            TickPlayer(time);
        }

        private void TickPlayer(float time)
        {
            _playerOperator.Tick(time);

            if (_playerOperator.Position.y < -5)
                _playerOperator.ResetState();
        }

        private void TickBlocks(float time)
        {
            _hectic = (_hectic + _hecticGain * time).UpperBound(_maxHectic);
            float offset = Velocity * time;
            _distance += offset;
            _distancePlanned += offset;

            foreach (BlockOperator blockOperator in _blockOperatorList)
                blockOperator.Move(Vector2.left * offset);

            while (_distancePlanned > 0)
            {
                List<bool> pickList = _blockMaker.PickNext();

                for (int i = 0; i < _blockRows; i++)
                {
                    if (pickList[i])
                    {
                        float y = 0 + _lowestRowY * i / _blockRows;
                        GameObject block = _blockPrefab.GeneratePrefab(_blockContainer);
                        BlockOperator blockOperator = block.GetComponent<BlockOperator>();
                        blockOperator.Setup(Vector2.up * y + Vector2.right * (_blockInsertDistance - _distancePlanned), _blockSize);
                        _blockOperatorList.Add(blockOperator);
                    }
                }

                _distancePlanned -= _blockSize;
            }

            for (int i = 0; i < _blockOperatorList.Count; i++)
            {
                BlockOperator blockOperator = _blockOperatorList[i];

                if (-blockOperator.Position.x > _blockDeleteDistance)
                {
                    _blockOperatorList.Remove(blockOperator);
                    blockOperator.Destroy();
                    i--;
                }
            }
        }
    }
}
