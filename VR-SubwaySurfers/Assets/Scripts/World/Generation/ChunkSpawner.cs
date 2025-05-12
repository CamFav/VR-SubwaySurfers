using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns building blocks in pairs, one on the left and one on the right.
/// </summary>
public class ChunkSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] chunkPrefabs;
    public Transform worldRoot;
    public BuildingBlockSpawner buildingSpawner;

    [Header("Settings")]
    public float chunkLength = 30f;
    public int chunksOnScreen = 5;
    public float spawnZOffset = 40f;
    public float despawnDistance = -80f;

    private float spawnZ;
    private readonly List<GameObject> activeChunks = new();

    void Start()
    {
        spawnZ = spawnZOffset;

        if (buildingSpawner == null)
            Debug.LogError("ChunkSpawner: buildingSpawner not assigned");

        var spawnChunk = GameObject.Find("Chunk_Spawn_Menu");
        if (spawnChunk) activeChunks.Add(spawnChunk);

        for (int i = 0; i < chunksOnScreen; i++)
            SpawnChunk();
    }

    void Update()
    {
        if (activeChunks.Count == 0) return;

        var first = activeChunks[0];
        if (first.transform.position.z + chunkLength < transform.position.z + despawnDistance)
        {
            Destroy(first);
            activeChunks.RemoveAt(0);

            SpawnChunk();
        }
    }

    private void SpawnChunk()
{
    int idx = Random.Range(0, chunkPrefabs.Length);
    GameObject chunk = Instantiate(chunkPrefabs[idx], worldRoot);
    chunk.transform.position = worldRoot.position + Vector3.forward * spawnZ;
    activeChunks.Add(chunk);

    spawnZ += chunkLength;

}


    private Vector3[] GetLanePositions(float z)
    {
        float laneDistance = 3.67f;
        return new Vector3[]
        {
            new Vector3(-laneDistance, 0f, z),
            new Vector3(0f, 0f, z),
            new Vector3(laneDistance, 0f, z)
        };
    }

}
