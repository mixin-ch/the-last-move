using Mixin.Utils;
using System.Collections;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    public GameObject collectablePrefab; // the collectable object to spawn
    [SerializeField]
    private GameObject _collectableContainer;
    public float spawnRadius = 50f; // the radius within which to spawn collectables
    public int spawnCount = 10; // the number of collectables to spawn
    public float minHeight = 5f; // the minimum height above the terrain to spawn collectables
    public float maxJumpRange = 5f; // the maximum jump range of the player
    public float spawnInterval = 10f; // the time interval between spawning collectables
    private float timeSinceLastSpawn; // the time since the last collectable was spawned
    private GameObject[] collectablePool; // the pool of collectable objects
    private int currentIndex; // the current index of the next collectable to use from the pool

    void Start()
    {
        // Initialize the collectable pool
        _collectableContainer.DestroyChildren();
        collectablePool = new GameObject[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            collectablePool[i] = collectablePrefab.GeneratePrefab(_collectableContainer);
            collectablePool[i].SetActive(false);
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
            for (int i = 0; i < spawnCount; i++)
            {
                // Get the next collectable from the pool
                var collectable = collectablePool[currentIndex];
                currentIndex = (currentIndex + 1) % spawnCount;

                // Generate a random position within the spawn radius
                var randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
                // Get the terrain height at the random position
                //var terrainHeight = Terrain.activeTerrain.SampleHeight(randomPos);
                var terrainHeight = -5;
                // Set the y-coordinate of the random position to be above the terrain height
                randomPos.y = terrainHeight + minHeight;
                // Ensure the collectable is reachable within the player's jump range
                if (randomPos.y - terrainHeight <= maxJumpRange)
                {
                    collectable.transform.position = randomPos;
                    collectable.SetActive(true);
                }
            }

            // Wait for the specified interval before spawning the next set of collectables
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
