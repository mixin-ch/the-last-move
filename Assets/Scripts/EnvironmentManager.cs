using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class EnvironmentManager : Singleton<EnvironmentManager>
    {
        [SerializeField]
        private GameObject _environmentContainer;
        [SerializeField]
        private GameObject _blockPrefab;

        private const float _blockSize = 2f;
        private const float _blockSpace = 2f;
        private const float _blockInsertDistance = 15f;
        private const float _blockDeleteDistance = 15f;
        private const float _startVelocity = 1f;
        private const float _acceleration = 0.1f;
        private const float _maxVelocity = 20f;

        private bool _active;
        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private float _velocity;
        private float _distance;
        private float _distancePlanned;

        private void OnEnable()
        {
            IngameUIB.OnPauseButtonClicked += PauseClicked;
        }

        private void OnDisable()
        {
            IngameUIB.OnPauseButtonClicked -= PauseClicked;
        }

        public void StartGame()
        {
            Clear();
            _active = true;
        }

        private void Clear()
        {
            _environmentContainer.DestroyChildren();
            _blockOperatorList.Clear();
            _velocity = _startVelocity;
            _distance = 0;
            _distancePlanned = _blockInsertDistance + _blockDeleteDistance;
        }

        private void PauseClicked()
        {
            _active = !_active;
        }

        private void FixedUpdate()
        {
            if (!_active)
                return;

            float time = Time.fixedDeltaTime;
            _velocity = (_velocity + _acceleration * time).UpperBound(_maxVelocity);
            float offset = _velocity * time;
            _distance += offset;
            _distancePlanned += offset;

            foreach (BlockOperator blockOperator in _blockOperatorList)
                blockOperator.Move(Vector2.left * offset);

            while (_distancePlanned > 0)
            {
                GameObject block = _blockPrefab.GeneratePrefab(_environmentContainer);
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
