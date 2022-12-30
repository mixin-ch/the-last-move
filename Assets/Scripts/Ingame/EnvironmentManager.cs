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

        private const float _blockSize = 2f;
        private const float _blockSpace = 2f;
        private const float _blockInsertDistance = 15f;
        private const float _blockDeleteDistance = 15f;
        private const float _startVelocity = 1f;
        private const float _acceleration = 0.1f;
        private const float _maxVelocity = 20f;

        private bool _started;
        private bool _paused;
        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private PlayerOperator _playerOperator;
        private float _velocity;
        private float _distance;
        private float _distancePlanned;

        public bool Started { get => _started; }
        public bool Paused { get => _paused; }

        private void OnEnable()
        {
            IngameUIB.OnPauseButtonClicked += PauseClicked;
            InputManager.OnJumpClicked += JumpClicked;
        }

        private void OnDisable()
        {
            IngameUIB.OnPauseButtonClicked -= PauseClicked;
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
            _velocity = _startVelocity;
            _distance = 0;
            _distancePlanned = _blockInsertDistance + _blockDeleteDistance;
        }

        private void PauseClicked()
        {
            if (!_started)
                return;

            _paused = !_paused;
            _playerOperator.Pause(_paused);
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
            _velocity = (_velocity + _acceleration * time).UpperBound(_maxVelocity);
            float offset = _velocity * time;
            _distance += offset;
            _distancePlanned += offset;

            foreach (BlockOperator blockOperator in _blockOperatorList)
                blockOperator.Move(Vector2.left * offset);

            while (_distancePlanned > 0)
            {
                GameObject block = _blockPrefab.GeneratePrefab(_blockContainer);
                BlockOperator blockOperator = block.GetComponent<BlockOperator>();
                blockOperator.Setup(Vector2.right * (_blockInsertDistance - _distancePlanned), _blockSize);
                _blockOperatorList.Add(blockOperator);
                _distancePlanned -= _blockSize + _blockSpace;
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
