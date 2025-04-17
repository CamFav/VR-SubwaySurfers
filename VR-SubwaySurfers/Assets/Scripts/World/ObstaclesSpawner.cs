using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawns obstacles randomly at designated spawn points.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")] // Prefabs to instantiate
    public GameObject obstacleSolidPrefab;
    public GameObject obstacleJumpablePrefab;
    public GameObject obstacleSlideablePrefab;

    [Header("Parameters")]
    [Range(0, 2)] public int maxObstacles = 3; // Maximum obstacles per chunk (0-3)

    void Start()
    {
        Transform obstaclePoints = transform.Find("Obstacles");
        if (obstaclePoints == null || obstaclePoints.childCount == 0)
        {
            Debug.LogWarning("No 'Obstacles'found in chunk.");
            return;
        }

        // List of possible spawn points
        List<Transform> availablePoints = new();
        for (int i = 0; i < obstaclePoints.childCount; i++)
        {
            availablePoints.Add(obstaclePoints.GetChild(i));
        }

        // Determine how many obstacles to spawn
        int obstacleCount = Random.Range(0, maxObstacles + 1);
        if (obstacleCount == 0) return;

        // Shuffle points to randomize spawn locations
        for (int i = 0; i < availablePoints.Count; i++)
        {
            int rand = Random.Range(i, availablePoints.Count);
            (availablePoints[i], availablePoints[rand]) = (availablePoints[rand], availablePoints[i]);
        }

        List<GameObject> selectedPrefabs = new();

        if (obstacleCount == 3)
        {
            // 3 obstacles
            // Randomly select one jumpable and one slideable obstacle
            int passableIndex = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
            {
                if (i == passableIndex)
                {
                    selectedPrefabs.Add(Random.value > 0.5f ? obstacleJumpablePrefab : obstacleSlideablePrefab);
                }
                else
                {
                    selectedPrefabs.Add(obstacleSolidPrefab);
                }
            }
        }
        else
        {
            // 1 or 2 obstacles
            // Randomly select the type of obstacles
            for (int i = 0; i < obstacleCount; i++)
            {
                float roll = Random.value;
                if (roll < 0.4f)
                    selectedPrefabs.Add(obstacleSolidPrefab);
                else if (roll < 0.7f)
                    selectedPrefabs.Add(obstacleJumpablePrefab);
                else
                    selectedPrefabs.Add(obstacleSlideablePrefab);
            }
        }

        // Instantiate the selected prefabs at the available points
        for (int i = 0; i < selectedPrefabs.Count; i++)
        {
            GameObject prefab = selectedPrefabs[i];
            Transform point = availablePoints[i];

            // Instantiate at world position and rotation
            Vector3 spawnPosition = new Vector3(point.position.x, prefab.transform.position.y, point.position.z);
            GameObject instance = Instantiate(prefab, spawnPosition, point.rotation);

            // Parent to the chunk
            instance.transform.SetParent(this.transform);
        }

    }
}
