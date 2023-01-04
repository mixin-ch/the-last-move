using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class ObstacleManager : Singleton<ObstacleManager>
    {
        [SerializeField]
        private GameObject _obstacleContainer;
        [SerializeField]
        private GameObject _obstaclePrefab;

        private const float _obstacleInterval = 10;
        private const float _obstacleInsertDistance = 15f;
        private const float _obstacleDeleteDistance = 15f;

        private List<ObstacleOperator> _obstacleOperatorList = new List<ObstacleOperator>();
        private float _distancePlanned;

        public void Setup()
        {
            Clear();
        }

        public void Tick(float distanceOffset)
        {
            foreach (ObstacleOperator obstacleOperatorin in _obstacleOperatorList)
                obstacleOperatorin.Move(Vector2.left * distanceOffset);

            _distancePlanned += distanceOffset;

            while (_distancePlanned > 0)
            {
                GameObject obstacle = _obstaclePrefab.GeneratePrefab(_obstacleContainer);
                ObstacleOperator obstacleOperator = obstacle.GetComponent<ObstacleOperator>();
                obstacleOperator.Setup(Vector2.right * (_obstacleInsertDistance - _distancePlanned), 1);
                _obstacleOperatorList.Add(obstacleOperator);

                _distancePlanned -= _obstacleInterval;
            }

            for (int i = 0; i < _obstacleOperatorList.Count; i++)
            {
                ObstacleOperator obstacleOperator = _obstacleOperatorList[i];

                if (-obstacleOperator.Position.x > _obstacleDeleteDistance)
                {
                    _obstacleOperatorList.Remove(obstacleOperator);
                    obstacleOperator.Destroy();
                    i--;
                }
            }
        }

        private void Clear()
        {
            _obstacleContainer.DestroyChildren();
            _obstacleOperatorList.Clear();
            _distancePlanned = 0;
        }
    }
}
