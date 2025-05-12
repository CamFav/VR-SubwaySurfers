using UnityEngine;

/// <summary>
/// Moves the attached objects backwards at a set speed.
/// </summary>
public class WorldMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    public float MoveSpeed => moveSpeed;

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    /// Sets the speed of the object movement.
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
