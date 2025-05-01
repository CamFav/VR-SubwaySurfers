using UnityEngine;

/// <summary>
/// Manages the game start process, including enabling movement and disabling the menu canvas.
/// </summary>
public class GameStartManager : MonoBehaviour
{
    [SerializeField] private WorldMover worldMover;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private VRMovementController movementController;
    [SerializeField] private GameObject hudCanvas;

    public void OnClickPlay()
    {
        worldMover.SetMoveSpeed(3f); // Set the speed of the world mover to 3f

        // Disable the menu canvas
        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        // Enable player movement
        if (movementController != null)
            movementController.EnableMovement();

        // Show HUD
        if (hudCanvas != null)
            hudCanvas.SetActive(true);

        // Start scoring
        ScoreManager.Instance.StartScoring();
    }
}
