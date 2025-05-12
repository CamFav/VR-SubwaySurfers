using UnityEngine;

/// <summary>
/// Tracks the player's position in the game world.
/// </summary>
public class PlayerTracker : MonoBehaviour
{
    public static PlayerTracker Instance { get; private set; }

    public Transform playerTransform;

    public float PlayerZ => playerTransform != null ? playerTransform.position.z : 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
