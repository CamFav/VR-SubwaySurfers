using UnityEngine;

/// <summary>
/// Handles player collisions with obstacles in the game.
/// </summary>
public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "ObstacleSolid")
        {
            Crash();
        }
        else if (tag == "ObstacleJumpable")
        {
            if (!VRMovementController.isJumping)
                Crash();
        }
        else if (tag == "ObstacleSlideable")
        {
            if (!VRMovementController.isSliding)
                Crash();
        }
    }

    private void Crash()
    {
        Debug.Log("Collision avec obstacle");
        AudioManager.Instance.PlaySfx(AudioManager.Instance.crashSfx);

        StopAllMovement();

        FindObjectOfType<GameOverManager>().TriggerGameOver();
    }

    private void StopAllMovement()
    {
        // Stop world movement
        var worldMover = FindObjectOfType<WorldMover>();
        if (worldMover != null)
            worldMover.SetMoveSpeed(0f);

        var buildingMover = FindObjectOfType<WorldMoverBuildings>();
        if (buildingMover != null)
            buildingMover.SetMoveSpeed(0f);

        // Stop player movement
        var controller = FindObjectOfType<VRMovementController>();
        if (controller != null)
            controller.DisableMovement();
    }
}
