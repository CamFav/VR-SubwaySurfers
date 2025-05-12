using UnityEngine;
using System.Collections.Generic;

public class WheelRotator : MonoBehaviour
{
    [Header("Auto-Detect Wheels")]
    public Transform[] wheels;

    [Header("Rotation Speed")]
    public float rotationSpeed = 180f;

    void Awake()
    {
        // automatically find wheels if not assigned
        if (wheels == null || wheels.Length == 0)
        {
            var list = new List<Transform>();
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                if (child.name.ToLower().Contains("wheel"))
                    list.Add(child);
            }
            wheels = list.ToArray();
        }
    }

    void Update()
    {
        float delta = rotationSpeed * Time.deltaTime;
        foreach (var w in wheels)
            if (w != null)
                w.Rotate(Vector3.right, delta, Space.Self);
    }
}
