using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CoinSpawner))]
// <summary>
// Spawns obstacles in a chunk based on spawn points and profiles.
// </summary>
public class ChunkObstacleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public int maxObstacles = 5;
    public float minZSpacing = 1f;
    public float analysisWindowSize = 6f;
    public float windowStep = 1f;

    private class SpawnData
    {
        public ObstacleProfile profile;
        public float z;
        public float laneX;
        public float length;
    }

    private List<SpawnData> selectedSpawns;
    private float[] lanes = { -3.67f, 0f, 3.67f };

    void Awake()
    {
        // 1. Collect spawn points
        var spawnPoints = GetComponentsInChildren<SpawnPoint>();
        Array.Sort(spawnPoints, (a, b) => a.rowID.CompareTo(b.rowID));

        // Determine obstacle placements
        selectedSpawns = new List<SpawnData>();
        var usedLanesPerRow = new HashSet<(int row, float laneX)>();

        foreach (var sp in spawnPoints)
        {
            if (selectedSpawns.Count >= maxObstacles)
                break;
            if (usedLanesPerRow.Contains((sp.rowID, sp.laneX)))
                continue;

            // Shuffle profiles
            var profiles = new List<ObstacleProfile>(sp.allowedProfiles);
            for (int i = 0; i < profiles.Count; i++)
            {
                int r = UnityEngine.Random.Range(i, profiles.Count);
                (profiles[i], profiles[r]) = (profiles[r], profiles[i]);
            }

            foreach (var prof in profiles)
            {
                var cand = new SpawnData {
                    profile = prof,
                    z = sp.transform.position.z,
                    laneX = sp.laneX,
                    length = prof.length
                };

                if (!Overlaps(cand, selectedSpawns) && !BlocksAllLanes(cand, selectedSpawns, lanes))
                {
                    selectedSpawns.Add(cand);
                    usedLanesPerRow.Add((sp.rowID, sp.laneX));
                    break;
                }
            }
        }

        // Instantiate obstacles
        foreach (var s in selectedSpawns)
        {
            Vector3 pos = new Vector3(s.laneX,
                s.profile.prefab.transform.position.y,
                s.z);
            Instantiate(s.profile.prefab, pos, Quaternion.identity, transform);
        }
    }

    void Start()
    {
        // Spawn coins after chunk has been positioned and obstacles in place
        var coinSpawner = GetComponent<CoinSpawner>();
        if (coinSpawner != null)
        {
            // Use spawnPoints positions for coin generation
            var spawnPoints = GetComponentsInChildren<SpawnPoint>();
            var worldPositions = new List<Vector3>();
            foreach (var sp in spawnPoints)
                worldPositions.Add(sp.transform.position);

            coinSpawner.SpawnCoins(worldPositions.ToArray());
        }
    }

    /// <summary>
    /// Check if the new spawn overlaps with existing spawns in the same lane.
    /// </summary>
    private bool Overlaps(SpawnData newSpawn, List<SpawnData> existing)
    {
        foreach (var e in existing)
        {
            if (e.laneX != newSpawn.laneX) continue;
            float gap = minZSpacing;
            float aMin = newSpawn.z - newSpawn.length / 2f - gap;
            float aMax = newSpawn.z + newSpawn.length / 2f + gap;
            float bMin = e.z - e.length / 2f;
            float bMax = e.z + e.length / 2f;
            if (aMax >= bMin && aMin <= bMax) return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the new spawn blocks all lanes in the analysis window.
    /// /// </summary>
    private bool BlocksAllLanes(SpawnData newSpawn, List<SpawnData> existing, float[] lanes)
    {
        var testList = new List<SpawnData>(existing) { newSpawn };
        float zMin = float.MaxValue, zMax = float.MinValue;
        foreach (var s in testList)
        {
            float a = s.z - s.length / 2f;
            float b = s.z + s.length / 2f;
            zMin = Mathf.Min(zMin, a);
            zMax = Mathf.Max(zMax, b);
        }

        for (float z = zMin; z <= zMax; z += windowStep)
        {
            float a = z, b = z + analysisWindowSize;
            var solidLanes = new HashSet<float>();
            foreach (var s in testList)
            {
                if (s.profile.category != ObstacleProfile.Category.Solid) continue;
                float sMin = s.z - s.length / 2f;
                float sMax = s.z + s.length / 2f;
                if (sMax >= a && sMin <= b) solidLanes.Add(s.laneX);
            }
            if (solidLanes.Contains(lanes[0]) && solidLanes.Contains(lanes[1]) && solidLanes.Contains(lanes[2]))
                return true;
        }
        return false;
    }
}
