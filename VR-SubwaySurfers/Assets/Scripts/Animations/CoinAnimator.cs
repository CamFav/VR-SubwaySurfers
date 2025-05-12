using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f;

    [Header("Floating")]
    public float floatAmplitude = 0.25f; // height max of the floating effect
    public float floatFrequency = 1f;  // speed of the floating effect

    private Vector3 startLocalPos;

    void Start()
    {
        // base position for calculating the floating effect
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Rotation around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);

        // Floating effect
        float newY = startLocalPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 localPos = startLocalPos;
        localPos.y = newY;
        transform.localPosition = localPos;
    }
}
