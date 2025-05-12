using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabLengthViewer : MonoBehaviour
{
    public GameObject prefabToMeasure;
    public float measuredLength;

    void Update()
    {
        if (prefabToMeasure != null)
        {
            Renderer[] renderers = prefabToMeasure.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                foreach (var r in renderers)
                    bounds.Encapsulate(r.bounds);

                measuredLength = bounds.size.z;
            }
        }
    }
}
