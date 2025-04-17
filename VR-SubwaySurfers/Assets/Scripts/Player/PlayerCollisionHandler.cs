using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Restarts the scene if the player collides with an obstacle.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("Collision with obstacle");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
