using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Tilemap tilemap;
    public int maxEnemyCount = 10;
    public float respawnDelay = 2f;
    public Camera mainCamera;

    [SerializeField]
    private int currentEnemyCount = 0;
    private Bounds spawnAreaBounds;


    private void Start()
    {
        CalculateSpawnAreaBounds(); 
        // Spawn initial enemies
   
                SpawnEnemy();
    }

    private void Update()
    {
        // Spawn a new enemy if the maximum count is not reached
        if (currentEnemyCount < maxEnemyCount )
        {
            SpawnEnemy();
        }
    }

    private void CalculateSpawnAreaBounds()
    {
        if (tilemap != null)
        {
            BoundsInt bounds = tilemap.cellBounds;
            Vector3Int minCellPos = bounds.min;
            Vector3Int maxCellPos = bounds.max;

            Vector3 minWorldPos = tilemap.CellToWorld(minCellPos);
            Vector3 maxWorldPos = tilemap.CellToWorld(maxCellPos);

            spawnAreaBounds = new Bounds((maxWorldPos + minWorldPos) * 0.5f, maxWorldPos - minWorldPos);
        }
    }

    private void SpawnEnemy()
    {
        // Get a random position within the spawn area
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Check if the spawn position is within the camera's sight
        if (!IsInCameraSight(spawnPosition))
        {
            // Instantiate the enemy at the spawn position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // Increment the enemy count
            currentEnemyCount++;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Calculate a random position within the spawn area bounds
        Vector3 randomPos = spawnAreaBounds.center + new Vector3(
            Random.Range(-spawnAreaBounds.extents.x, spawnAreaBounds.extents.x),
            Random.Range(-spawnAreaBounds.extents.y, spawnAreaBounds.extents.y),
            0f);
        randomPos.z = tilemap.transform.position.z - 1f;
        return randomPos;
    }

    private bool IsInCameraSight(Vector3 position)
    {
        // Check if the position is within the camera's sight
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(position);
        return viewportPoint.x > 0f && viewportPoint.x < 1f && viewportPoint.y > 0f && viewportPoint.y < 1f;
    }

    
}
