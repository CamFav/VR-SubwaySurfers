using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private WorldMover worldMover;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private VRMovementController movementController;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private WorldMoverBuildings buildingMover;
    
    [Header("Ray Interaction")]
    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private XRInteractorLineVisual rightRayVisual;

    public void OnClickPlay()
    {
        worldMover.SetMoveSpeed(8f);

        if (menuCanvas != null)
            menuCanvas.SetActive(false);

        if (movementController != null)
            movementController.EnableMovement();

        if (hudCanvas != null)
            hudCanvas.SetActive(true);

        if (buildingMover != null)
            buildingMover.SetMoveSpeed(3.5f);

        if (rightRayInteractor != null)
            rightRayInteractor.enabled = false;

        if (rightRayVisual != null)
            rightRayVisual.enabled = false;

        ScoreManager.Instance.StartScoring();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
    }
}
