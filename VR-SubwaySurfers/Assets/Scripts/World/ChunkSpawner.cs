using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamically generates map chunks.
/// Maintains chunks ahead of the player.
/// Destroys chunks that fall too far behind.
/// </summary>
public class ChunkSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] chunkPrefabs;     // Different chunk types
    public Transform worldRoot;           // Parent object for spawned chunks

    [Header("Settings")]
    public float chunkLength = 20f;       // Z-axis length of each chunk
    public int chunksOnScreen = 5;        // Number of visible chunks
    public float spawnZOffset = 10f;      // Initial spawn position offset
    public float despawnDistance = -30f;  // Distance to destroy chunks behind player

    private float spawnZ = 0f;
    private List<GameObject> activeChunks = new();

    void Start()
    {
        // Initial spawn position
        spawnZ = spawnZOffset;

        // Generate initial visible chunks
        for (int i = 0; i < chunksOnScreen; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // Check if first chunk has moved behind player
        if (activeChunks.Count > 0)
        {
            GameObject firstChunk = activeChunks[0];

            if (firstChunk.transform.position.z + chunkLength < transform.position.z + despawnDistance)
            {
                Destroy(firstChunk);
                activeChunks.RemoveAt(0);
                SpawnChunk(); // Add new chunk ahead
            }
        }
    }

    void SpawnChunk()
    {
        int prefabIndex = Random.Range(0, chunkPrefabs.Length);
        GameObject chunk = Instantiate(chunkPrefabs[prefabIndex], worldRoot);

        // Set chunk position relative to worldRoot
        Vector3 spawnPosition = worldRoot.position + new Vector3(0, 0, spawnZ);
        chunk.transform.position = spawnPosition;

        // Update tracking values
        spawnZ += chunkLength;
        activeChunks.Add(chunk);
    }
}
