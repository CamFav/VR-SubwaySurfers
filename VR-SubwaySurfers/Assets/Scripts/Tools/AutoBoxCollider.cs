using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
/// <summary>
/// Automatically adjusts the BoxCollider to fit the bounds of the attached renderers.
/// </summary>
public class AutoBoxCollider : MonoBehaviour
{
    void Start()
    {
        UpdateColliderToFitVisuals();
    }

    public void UpdateColliderToFitVisuals()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return;
        }

        // Calcul of the bounds of all renderers
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        // Convert the bounds to local space
        Vector3 center = transform.InverseTransformPoint(bounds.center);
        Vector3 size = transform.InverseTransformVector(bounds.size);

        collider.center = center;
        collider.size = size;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying == false)
            UpdateColliderToFitVisuals();
    }
#endif
}
