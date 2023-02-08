using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField]
        private GameObject _blockContainer;
        [SerializeField]
        private GameObject _blockPrefab;
        [SerializeField]
        private GameObject _obstacleContainer;
        [SerializeField]
        private GameObject _obstaclePrefab;

        private const float _blockSize = 4f;
        private const float _insertDistance = 15f;
        private const float _deleteDistance = 15f;
        private const float _maxInsertHeight = 1f;
        private const float _minInsertHeight = -3f;
        private const float _startBlockHeightFraction = 0.5f;

        private ObjectPooler _op;

        private MapGenerator _mapGenerator;

        private int _blockPoolIndex;
        private int _obstaclePoolIndex;

        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private List<ObstacleOperator> _obstacleOperatorList = new List<ObstacleOperator>();
        private float _distancePlanned;

        public static float StartBlockHeightFraction => _startBlockHeightFraction;

        private void Start()
        {
            _blockContainer.DestroyChildren();
            _obstacleContainer.DestroyChildren();
            _op = ObjectPooler.SharedInstance;
            _blockPoolIndex = _op.AddObject(_blockPrefab);
            _obstaclePoolIndex = _op.AddObject(_obstaclePrefab);
        }

        public void Clear()
        {
            while (_blockOperatorList.Count > 0)
            {
                _blockOperatorList[0].gameObject.SetActive(false);
                _blockOperatorList.RemoveAt(0);
            }

            while (_obstacleOperatorList.Count > 0)
            {
                _obstacleOperatorList[0].gameObject.SetActive(false);
                _obstacleOperatorList.RemoveAt(0);
            }

            _distancePlanned = 25;

            _mapGenerator = new MapGenerator(EnvironmentManager.Instance.Hectic / _blockSize, 1);
        }

        public void Tick(float offset)
        {
            TickBlocks(offset);
            TickObstacles(offset);
            TickMapGeneration(offset);
        }

        public void KillObstacle(ObstacleOperator obstacleOperator)
        {
            if (!EnvironmentManager.Instance.Started)
                return;

            _obstacleOperatorList.Remove(obstacleOperator);
            //Destroy(obstacleOperator.gameObject);
            obstacleOperator.gameObject.SetActive(false);
        }


        private void TickBlocks(float offset)
        {
            //foreach (BlockOperator @operator in _blockOperatorList)
            //    @operator.Move(Vector2.left * offset);

            _blockContainer.transform.position += (Vector3)Vector2.left * offset;

            for (int i = 0; i < _blockOperatorList.Count; i++)
            {
                BlockOperator @operator = _blockOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _blockOperatorList.Remove(@operator);
                    //@operator.Destroy();
                    @operator.gameObject.SetActive(false);
                    i--;
                }
            }
        }

        private void TickObstacles(float offset)
        {
            //foreach (ObstacleOperator @operator in _obstacleOperatorList)
            //    @operator.Move(Vector2.left * offset);

            _obstacleContainer.transform.position += (Vector3)Vector2.left * offset;

            for (int i = 0; i < _obstacleOperatorList.Count; i++)
            {
                ObstacleOperator @operator = _obstacleOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _obstacleOperatorList.Remove(@operator);
                    //Destroy(@operator.gameObject);
                    @operator.gameObject.SetActive(false);
                    i--;
                }
            }
        }

        private void TickMapGeneration(float offset)
        {
            _distancePlanned += offset;

            while (_distancePlanned > 0)
            {
                _mapGenerator.BlockChunkSize = EnvironmentManager.Instance.Hectic / _blockSize;
                PlaceMapPlan(_mapGenerator.Tick(), _insertDistance - _distancePlanned);
                _distancePlanned -= _blockSize;
            }
        }

        private void PlaceMapPlan(MapPlan mapPlan, float x)
        {
            foreach (BlockPlan plan in mapPlan.BlockPlanList)
            {
                GameObject gameObject = _op.GetPooledObject(_blockPoolIndex);
                gameObject.transform.SetParent(_blockContainer.transform);
                //GameObject gameObject = _blockPrefab.GeneratePrefab(_blockContainer);
                BlockOperator @operator = gameObject.GetComponent<BlockOperator>();
                float y = Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(new Vector3(x, y, y * 0.1f), _blockSize, EnvironmentManager.Instance.CurrentBiome);
                _blockOperatorList.Add(@operator);
                gameObject.SetActive(true);
            }

            foreach (ObstaclePlan plan in mapPlan.ObstaclePlanList)
            {
                GameObject gameObject = _op.GetPooledObject(_obstaclePoolIndex);
                gameObject.transform.SetParent(_obstacleContainer.transform);
                //GameObject gameObject = _obstaclePrefab.GeneratePrefab(_obstacleContainer);
                ObstacleOperator @operator = gameObject.GetComponent<ObstacleOperator>();
                float y = _blockSize * 0.5f + Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(new Vector3(x, y, y * 0.1f));
                _obstacleOperatorList.Add(@operator);
                gameObject.SetActive(true);
            }
        }
    }
}
