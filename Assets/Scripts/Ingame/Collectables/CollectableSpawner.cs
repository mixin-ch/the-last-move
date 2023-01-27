using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mixin.TheLastMove.Environment.Collectable
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _collectablePrefab; // the collectable object to spawn
        [SerializeField]
        private GameObject _collectableContainer;
        [SerializeField]
        private float _spawnRadius = 50f; // the radius within which to spawn collectables
        [SerializeField]
        private int _spawnCount = 10; // the number of collectables to spawn
        [SerializeField]
        private float _minHeight = -5f; // the minimum height above the terrain to spawn collectables
        [SerializeField]
        private float _maxJumpRange = 5f; // the maximum jump range of the player
        [SerializeField]
        private float _spawnInterval = 10f; // the time interval between spawning collectables

        private float _timeSinceLastSpawn; // the time since the last collectable was spawned
        private GameObject[] _collectablePool; // the pool of collectable objects
        private int _currentIndex; // the current index of the next collectable to use from the pool

        private List<Collectable> _collectableComponents = new List<Collectable>();
        private int _currentCollectableIndex; // the current index of the next collectable component to use from the list

        private float _moveSpeed; // The speed of the terrain movement
        private float _moveDistance; // The distance to move the terrain

        void Start()
        {
            // Initialize the collectable pool
            _collectableContainer.DestroyChildren();
            _collectablePool = new GameObject[_spawnCount];
            for (int i = 0; i < _spawnCount; i++)
            {
                _collectablePool[i] = _collectablePrefab.GeneratePrefab(_collectableContainer);

                //Save the reference to the Collectable component
                Collectable collectableComponent = _collectablePool[i].GetComponent<Collectable>();
                _collectableComponents.Add(collectableComponent);

                _collectableComponents[i].Deactivate();
            }

            // Start spawning collectables
            StartCoroutine(SpawnCollectables());
        }

        private IEnumerator SpawnCollectables()
        {
            while (true)
            {
                for (int i = 0; i < _spawnCount; i++)
                {
                    var collectable = _collectablePool[_currentIndex];
                    _currentIndex = (_currentIndex + 1) % _spawnCount;

                    var randomPos = new Vector3(transform.position.x + Random.Range(-_spawnRadius, _spawnRadius),
                                                transform.position.y + Random.Range(-_spawnRadius, _spawnRadius),
                                                0);

                    if (randomPos.y - _minHeight <= _maxJumpRange)
                    {
                        collectable.transform.position = randomPos;
                        //Activate the collectable
                        _collectableComponents[_currentCollectableIndex].Activate();
                        _currentCollectableIndex = (_currentCollectableIndex + 1) % _spawnCount;
                    }
                }
                yield return new WaitForSeconds(_spawnInterval);
            }
        }


        public void MoveCollectablesWithTerrain(float distance)
        {
            StartCoroutine(MoveCollectables(distance));
        }

        private IEnumerator MoveCollectables(float distance)
        {
            //Move the terrain and collectables
            for (int i = 0; i < _spawnCount; i++)
            {
                if (_collectablePool[i].activeSelf)
                {
                    _collectablePool[i].transform.position += new Vector3(distance * -1, 0, 0);
                }
            }
            yield return new WaitForSeconds(_moveSpeed);
        }
    }
}
