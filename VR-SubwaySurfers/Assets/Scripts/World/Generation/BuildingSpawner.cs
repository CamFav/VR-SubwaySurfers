using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns building blocks in pairs, one on the left and one on the right.
/// </summary>
public class BuildingBlockSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] leftPrefabs;
    public GameObject[] rightPrefabs;

    [Header("References")]
    public Transform worldBuildings;
    public Transform generationOrigin;

    [Header("Paramètres")]
    public int    initialBlocksCount = 12;
    public float  spawnZOffset       = 0f;
    public float  despawnDistance    = -40f;
    public float  lateralOffset      = 6f;

    private readonly List<GameObject> leftBlocks  = new();
    private readonly List<GameObject> rightBlocks = new();

    void Start()
    {
        for (int i = 0; i < initialBlocksCount; i++)
            SpawnBlockPair();
    }

    void Update()
    {
        while (leftBlocks.Count > 0 &&
               leftBlocks[0].transform.position.z < generationOrigin.position.z + despawnDistance)
        {
            Destroy(leftBlocks[0]);
            Destroy(rightBlocks[0]);
            leftBlocks.RemoveAt(0);
            rightBlocks.RemoveAt(0);

            SpawnBlockPair();
        }
    }

    private void SpawnBlockPair()
    {
        // Calcul of the previous max.z position
        float prevMaxZ;
        if (leftBlocks.Count == 0)
        {
            prevMaxZ = generationOrigin.position.z + spawnZOffset;
        }
        else
        {
            var last = leftBlocks[leftBlocks.Count - 1];
            var rends = last.GetComponentsInChildren<Renderer>();
            Bounds b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
            prevMaxZ = b.max.z;
        }

        // Random prefabs
        var leftPrefab  = leftPrefabs[Random.Range(0, leftPrefabs.Length)];
        var rightPrefab = rightPrefabs[Random.Range(0, rightPrefabs.Length)];

        // Calcul of the min.z of the prefab
        float leftMinLocalZ  = CalculateLocalMinZ(leftPrefab);
        float rightMinLocalZ = CalculateLocalMinZ(rightPrefab);

        // position de spawn pour que prefab.minZ == prevMaxZ
        float spawnZLeft  = prevMaxZ - leftMinLocalZ;
        float spawnZRight = prevMaxZ - rightMinLocalZ;

        Vector3 origin = generationOrigin.position;

        var left = Instantiate(
            leftPrefab,
            origin + new Vector3(-lateralOffset, 0f, spawnZLeft),
            Quaternion.identity,
            worldBuildings
        );
        leftBlocks.Add(left);

        var right = Instantiate(
            rightPrefab,
            origin + new Vector3( lateralOffset, 0f, spawnZRight),
            Quaternion.identity,
            worldBuildings
        );
        rightBlocks.Add(right);
    }

    // calcule minZ local du bounds d'un prefab
    private float CalculateLocalMinZ(GameObject prefab)
    {
        // instanciation temporaire hors écran
        var temp = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        var rends = temp.GetComponentsInChildren<Renderer>();
        Bounds b = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);

        // conversion back to local min.z
        float worldMinZ = b.min.z;
        float localMinZ = temp.transform.InverseTransformPoint(new Vector3(0,0,worldMinZ)).z;
        Destroy(temp);
        return localMinZ;
    }
}
