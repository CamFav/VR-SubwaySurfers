using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns coin lines at designated spawn points.
/// </summary>
public class CoinLineSpawner : MonoBehaviour
{
    [Header("Coin Line Prefab")]
    public GameObject coinLinePrefab;

    [Header("Settings")]
    public int maxLines = 2;

    void Start()
    {
        Transform coinPoints = transform.Find("Coins");
        Transform obstaclePoints = transform.Find("Obstacles");

        if (coinPoints == null || obstaclePoints == null)
            return;

        List<Transform> available = new();

        // Check for available coin points
        // with no obstacles in the same position
        for (int i = 0; i < coinPoints.childCount; i++)
        {
            Transform coinPoint = coinPoints.GetChild(i);
            string expectedObstacleName = coinPoint.name.Replace("Coin_", "Obstacle_");
            Transform obstaclePoint = obstaclePoints.Find(expectedObstacleName);

            if (obstaclePoint != null && obstaclePoint.childCount == 0)
            {
                available.Add(coinPoint);
            }
        }

        int linesToSpawn = Mathf.Min(maxLines, available.Count);

        for (int i = 0; i < linesToSpawn; i++)
        {
            Vector3 pos = available[i].position;
            Instantiate(coinLinePrefab, pos, Quaternion.identity, transform);
        }
    }
}
