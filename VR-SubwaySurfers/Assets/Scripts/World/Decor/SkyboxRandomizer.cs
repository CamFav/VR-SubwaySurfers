using UnityEngine;

/// <summary>
/// Randomly selects and applies a skybox material at game start or scene reload.
/// </summary>
public class SkyboxRandomizer : MonoBehaviour
{ 
    public Material[] skyboxMaterials;

    public bool updateEnvironment = true;

    void Awake()
    {
        ApplyRandomSkybox();
    }

    /// <summary>
    /// Picks a random skybox
    /// </summary>
    public void ApplyRandomSkybox()
    {
        if (skyboxMaterials == null || skyboxMaterials.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, skyboxMaterials.Length);
        RenderSettings.skybox = skyboxMaterials[index];

        #if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
        #endif

        if (updateEnvironment)
        {
            // Updates ambient lighting and sky reflections
            DynamicGI.UpdateEnvironment();
        }

        Debug.Log($"Applied skybox '{skyboxMaterials[index].name}'");
    }
}
