using UnityEngine;

/// <summary>
/// Spawn coins at the given world positions, then attach them to this chunk
/// so they move with it during worldroot movement.
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public float yOffsetDefault = 0.5f;
    public float yOffsetJumpable = 1.5f;
    public float raycastHeight = 2f;
    public float raycastDistance = 3f;

    public void SpawnCoins(Vector3[] lanePositions)
    {

        Transform chunkRoot = transform;

        foreach (Vector3 worldPos in lanePositions)
        {

            Vector3 rayStart = new Vector3(worldPos.x, worldPos.y + raycastHeight, worldPos.z);
            float y;
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, raycastDistance))
            {
                if (hit.collider.CompareTag("ObstacleSolid"))
                    continue; // pas de coin sur un obstacle solide
                y = hit.collider.CompareTag("ObstacleJumpable") ? yOffsetJumpable : yOffsetDefault;
            }
            else
            {
                y = yOffsetDefault;
            }

            // Calcul de la position monde où spawner le coin
            Vector3 spawnWorldPos = new Vector3(worldPos.x, y, worldPos.z);

            GameObject coin = Instantiate(coinPrefab);

            coin.transform.SetParent(chunkRoot, false);

            // Convertir position monde en local du chunk
            Vector3 localPos = chunkRoot.InverseTransformPoint(spawnWorldPos);
            coin.transform.localPosition = localPos;
            coin.transform.localRotation = Quaternion.identity;
        }
    }
}
