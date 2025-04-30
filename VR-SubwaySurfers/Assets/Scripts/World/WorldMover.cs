using UnityEngine;

/// <summary>
///  Moves the attached objects backwards at a set speed.
/// </summary>
public class WorldMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // Speed of the object movement

    void Update()
    {
        // Move the object backwards at the specified speed
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    /// Sets the speed of the object movement.     
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
