using UnityEngine;

public class RoadTile : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs;    // assign multiple obstacle prefabs in Inspector
    [Range(0f, 1f)]
    public float obstacleChance = 0.5f;     // chance per lane

    [Header("Lane Settings")]
    public float laneDistance = 3f;         // distance between lanes
    [HideInInspector]
    public bool allowObstacles = true;      // set false for first tile

    private string[] lanes = { "Spawn_Left", "Spawn_Middle", "Spawn_Right" };

    // Call this after instantiating the tile
    public void SpawnObstacles()
    {
        // Early exit if obstacles are not allowed
        if (!allowObstacles)
        {
            Debug.Log("Obstacles not allowed on this tile: " + name);
            return;
        }

        // Early exit if no obstacle prefabs assigned
        if (obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("No obstacle prefabs assigned in tile: " + name);
            return;
        }

        Debug.Log("Spawning obstacles for tile: " + name);

        bool[] laneHasObstacle = new bool[3];
        int obstaclesThisTile;

        // Ensure at least one lane is free
        do
        {
            obstaclesThisTile = 0;
            for (int i = 0; i < lanes.Length; i++)
            {
                laneHasObstacle[i] = Random.value < obstacleChance;
                if (laneHasObstacle[i])
                    obstaclesThisTile++;
            }
        } while (obstaclesThisTile == 3);

        // Spawn obstacles
        for (int i = 0; i < lanes.Length; i++)
        {
            if (!laneHasObstacle[i])
                continue;

            Transform spawnPoint = transform.Find(lanes[i]);
            if (spawnPoint == null)
            {
                Debug.LogWarning("Missing spawn point: " + lanes[i] + " in tile: " + name);
                continue;
            }

            Vector3 pos = spawnPoint.position;
            pos.x = transform.position.x + (i - 1) * laneDistance;

            // Pick random obstacle from array
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(prefab, pos, prefab.transform.rotation, transform);

            Debug.Log("Spawned obstacle: " + prefab.name + " at lane: " + lanes[i]);
        }
    }
}


