using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float initialSpawnCount = 25f;
    public float spawnIncreaseRate = 2f;
    public Tilemap obstacleTilemap;
    public Player playerScript;

    public float currentSpawnCount = 0;
    private Bounds tilemapBounds;



    public int newSpawnCount;
    private void Start()
    {
        CalculateTilemapBounds();
    }

    private void CalculateTilemapBounds()
    {
        tilemapBounds = obstacleTilemap.localBounds;
    }

   
    private Vector3 GetRandomSpawnPosition()
    {

        Vector3 randomPosition = new Vector3(
            Random.Range(tilemapBounds.min.x, tilemapBounds.max.x),
            Random.Range(tilemapBounds.min.y, tilemapBounds.max.y),
            0f
        );

        if (IsPositionValid(randomPosition))
            return randomPosition;

        return GetRandomSpawnPosition();
    }

    private bool IsPositionValid(Vector3 position)
    {
        Vector3Int cellPosition = obstacleTilemap.WorldToCell(position);
        TileBase tile = obstacleTilemap.GetTile(cellPosition);

        // Check if the position is within the tilemap and not on an obstacle tile
        return tile == null;
    }

    private void Update()
    {
        int playerLevel = playerScript.currentLevel;
        long totalMemory = Profiler.GetTotalReservedMemoryLong();
        long usedMemory = Profiler.GetTotalAllocatedMemoryLong();
        long availableMemory = totalMemory - usedMemory;

        long memoryThreshold = 200 * 1024 * 1024;

        newSpawnCount = Mathf.RoundToInt(initialSpawnCount * Mathf.Pow(spawnIncreaseRate, playerLevel - 1));

        if (availableMemory < memoryThreshold)
        {
            newSpawnCount = 0;
        }

        if (newSpawnCount > currentSpawnCount)
        {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            currentSpawnCount++;
        }
    }

    public void UpdateCurrentEnemy()
    {
        currentSpawnCount--;
    }
}

