using UnityEngine;

/// <summary>
/// Spawns obstacles randomly at designated spawn points.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject obstaclePrefab; // Prefab to instantiate

    void Start()
    {
        Transform obstaclePoints = transform.Find("Obstacles");
        if (obstaclePoints == null)
        {
            Debug.LogWarning("No 'Obstacles'found in chunk.");
            return;
        }

        // Get all child points (Left, Center, Right)
        int childCount = obstaclePoints.childCount;
        if (childCount == 0) return;

        int randomIndex = Random.Range(0, childCount);
        Transform spawnPoint = obstaclePoints.GetChild(randomIndex);

        // Instantiate obstacle at selected position
        Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
    }
}
