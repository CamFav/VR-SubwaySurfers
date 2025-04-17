using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Restarts the scene if the player collides with an obstacle.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "ObstacleSolid")
        {
            Restart();
        }
        else if (tag == "ObstacleJumpable")
        {
            if (!VRMovementController.isJumping)
                Restart();
        }
        else if (tag == "ObstacleSlideable")
        {
            if (!VRMovementController.isSliding)
                Restart();
        }
    }

    private void Restart()
    {
        Debug.Log("Collision with obstacle.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
