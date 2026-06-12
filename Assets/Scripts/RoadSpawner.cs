using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject roadTilePrefab;

    [Header("Settings")]
    public int initialTiles = 5;          // number of tiles to spawn at start
    public float spawnDistanceAhead = 50f; // distance before spawning next tile

    private List<GameObject> activeTiles = new List<GameObject>();
    private Vector3 nextSpawnPoint;
    private bool firstTileSpawned = false;

    void Start()
    {
        if (player == null)
            Debug.LogError("Player not assigned in RoadSpawner!");

        if (roadTilePrefab == null)
            Debug.LogError("RoadTile prefab not assigned in RoadSpawner!");

        nextSpawnPoint = transform.position;

        // Spawn initial tiles
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Spawn a new tile if player is close to the end
        if (player.position.z + spawnDistanceAhead > nextSpawnPoint.z)
        {
            SpawnTile();
            RemoveOldTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(roadTilePrefab, nextSpawnPoint, Quaternion.identity);
        RoadTile rt = tile.GetComponent<RoadTile>();

        // Disable obstacles on the first tile
        rt.allowObstacles = firstTileSpawned;
        firstTileSpawned = true;

        // Call obstacle spawn manually
        rt.SpawnObstacles();

        activeTiles.Add(tile);

        // Update nextSpawnPoint using the tile's NextSpawnPoint
        Transform tileEnd = tile.transform.Find("NextSpawnPoint");
        if (tileEnd != null)
            nextSpawnPoint = tileEnd.position;
        else
            nextSpawnPoint += new Vector3(0, 0, 30); // fallback if marker missing

        Debug.Log("Spawned tile: " + tile.name + " | Next spawn point: " + nextSpawnPoint);
    }

    void RemoveOldTile()
    {
        if (activeTiles.Count > initialTiles)
        {
            Destroy(activeTiles[0]);
            activeTiles.RemoveAt(0);
        }
    }
}


