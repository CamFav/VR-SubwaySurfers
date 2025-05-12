using UnityEngine;

/// <summary>
/// This class is responsible for moving the buildings in the game.
/// </summary>
public class WorldMoverBuildings : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    public float MoveSpeed => moveSpeed;

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    /// Sets the speed of the object movement.
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
