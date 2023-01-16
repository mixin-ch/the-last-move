using Mixin.Utils;
using System.Collections;
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
        private float _minHeight = 5f; // the minimum height above the terrain to spawn collectables
        [SerializeField]
        private float _maxJumpRange = 5f; // the maximum jump range of the player
        [SerializeField]
        private float _spawnInterval = 10f; // the time interval between spawning collectables

        private float _timeSinceLastSpawn; // the time since the last collectable was spawned
        private GameObject[] _collectablePool; // the pool of collectable objects
        private int _currentIndex; // the current index of the next collectable to use from the pool

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
                _collectablePool[i].SetActive(false);
            }

            // Start spawning collectables
            StartCoroutine(SpawnCollectables());
        }

        private IEnumerator SpawnCollectables()
        {
            while (true)
            {
                // Get the terrain collider component
                //var terrainCollider = GetComponent<TerrainCollider>();

                // Spawn collectables at random positions within the spawn radius
                for (int i = 0; i < _spawnCount; i++)
                {
                    // Get the next collectable from the pool
                    var collectable = _collectablePool[_currentIndex];
                    _currentIndex = (_currentIndex + 1) % _spawnCount;

                    // Generate a random position within the spawn radius
                    var randomPos = transform.position + Random.insideUnitSphere * _spawnRadius;
                    // Get the terrain height at the random position
                    //var terrainHeight = Terrain.activeTerrain.SampleHeight(randomPos);
                    var terrainHeight = -5;
                    // Set the y-coordinate of the random position to be above the terrain height
                    randomPos.y = terrainHeight + _minHeight;
                    // Ensure the collectable is reachable within the player's jump range
                    if (randomPos.y - terrainHeight <= _maxJumpRange)
                    {
                        collectable.transform.position = randomPos;
                        collectable.SetActive(true);
                    }
                }

                // Wait for the specified interval before spawning the next set of collectables
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
