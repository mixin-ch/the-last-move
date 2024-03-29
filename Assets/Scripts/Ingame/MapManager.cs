using Mixin.TheLastMove.Environment.Collectable;
using Mixin.TheLastMove.Utils;
using Mixin.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField]
        private GameObject _movingContainer;
        [SerializeField]
        private GameObject _blockContainer;
        [SerializeField]
        private GameObject _blockPrefab;
        [SerializeField]
        private GameObject _obstacleContainer;
        [SerializeField]
        private GameObject _obstaclePrefab;
        [SerializeField]
        private GameObject _collectableContainer;
        [SerializeField]
        private GameObject _collectablePrefab;

        [SerializeField]
        private float _obstacleProbability;
        [SerializeField]
        private List<KeyAndValueLocked<ObstacleOperator, float>> _obstacleWeightDict;
        [SerializeField]
        private float _collectableProbability;
        [SerializeField]
        private List<KeyAndValueLocked<CollectableDataSet, float>> _collectableWeightDict;

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
        private int _collectablePoolIndex;

        private List<BlockOperator> _blockOperatorList = new List<BlockOperator>();
        private List<ObstacleOperator> _obstacleOperatorList = new List<ObstacleOperator>();
        private List<CollectableOperator> _collectableOperatorList = new List<CollectableOperator>();
        private float _distancePlanned;

        public static float StartBlockHeightFraction => _startBlockHeightFraction;

        private void Start()
        {
            _blockContainer.DestroyChildren();
            _obstacleContainer.DestroyChildren();
            _collectableContainer.DestroyChildren();

            _op = ObjectPooler.SharedInstance;

            _blockPoolIndex = _op.AddObject(_blockPrefab);
            _obstaclePoolIndex = _op.AddObject(_obstaclePrefab);
            _collectablePoolIndex = _op.AddObject(_collectablePrefab);
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

            while (_collectableOperatorList.Count > 0)
            {
                _collectableOperatorList[0].gameObject.SetActive(false);
                _collectableOperatorList.RemoveAt(0);
            }

            _distancePlanned = 25;
            _mapGenerator = new MapGenerator(EnvironmentManager.Instance.Hectic / _blockSize, 1
                , _obstacleProbability, _collectableProbability
                , _obstacleWeightDict.ToDictionary(), _collectableWeightDict.ToDictionary());
        }

        public void Tick(float offset)
        {
            _movingContainer.transform.position += (Vector3)Vector2.left * offset;
            TickBlocks();
            TickObstacles();
            TickCollectables();
            TickMapGeneration(offset);
        }

        public void KillObstacle(ObstacleOperator obstacleOperator)
        {
            if (!EnvironmentManager.Instance.Started)
                return;

            _obstacleOperatorList.Remove(obstacleOperator);
            obstacleOperator.gameObject.SetActive(false);
        }


        private void TickBlocks()
        {
            for (int i = 0; i < _blockOperatorList.Count; i++)
            {
                BlockOperator @operator = _blockOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _blockOperatorList.Remove(@operator);
                    @operator.gameObject.SetActive(false);
                    i--;
                }
            }
        }

        private void TickObstacles()
        {
            for (int i = 0; i < _obstacleOperatorList.Count; i++)
            {
                ObstacleOperator @operator = _obstacleOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _obstacleOperatorList.Remove(@operator);
                    @operator.gameObject.SetActive(false);
                    i--;
                }
            }
        }

        private void TickCollectables()
        {
            for (int i = 0; i < _collectableOperatorList.Count; i++)
            {
                CollectableOperator @operator = _collectableOperatorList[i];

                if (-@operator.Position.x > _deleteDistance)
                {
                    _collectableOperatorList.Remove(@operator);
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
                ObstacleOperator @operator = gameObject.GetComponent<ObstacleOperator>();
                float y = Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(plan.Obstacle, new Vector3(x, y, y * 0.1f));
                _obstacleOperatorList.Add(@operator);
                gameObject.SetActive(true);
            }
            foreach (CollectablePlan plan in mapPlan.CollectablePlanList)
            {
                GameObject gameObject = _op.GetPooledObject(_collectablePoolIndex);
                gameObject.transform.SetParent(_collectableContainer.transform);
                CollectableOperator @operator = gameObject.GetComponent<CollectableOperator>();
                float y = Mathf.Lerp(_minInsertHeight, _maxInsertHeight, plan.Height);
                @operator.Setup(plan.Collectable, new Vector3(x, y, y * 0.1f) + (Vector3)plan.Offset);
                _collectableOperatorList.Add(@operator);
                gameObject.SetActive(true);
                @operator.MakeStart();
            }
        }
    }
}
