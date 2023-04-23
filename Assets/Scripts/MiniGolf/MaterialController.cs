using System;
using UnityEngine;

/// <summary>
/// Utility component that just updates a list of materials each frame with a particular parameter
/// </summary>
public class MaterialController : MonoBehaviour
{
    /// <summary>
    /// Creates instances of each material in the renderers so that they can be changed without affecting the actual materials
    /// </summary>
    /// <param name="renderers">Array of renderers to change the materials of</param>
    /// <returns>Array of all newly created temporary materials</returns>
    public static Material[] InstantiateMaterials(params Renderer[] renderers)
    {
        Material[] materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = new Material(renderers[i].sharedMaterial);
            renderers[i].sharedMaterial = materials[i];
        }
        return materials;
    }

    // Parameter name in the Material that is changed
    [SerializeField]
    private string parameterName;

    // Current parameter value in the Material
    [SerializeField]
    private float parameterValue;

    // Array of renderers to update every frame
    [SerializeField]
    private Renderer[] renderers;

    // Array of all Materials to update every frame
    private Material[] materials;

    /// <summary>
    /// To initialize, create instances of all controlled materials so that changes do not affect the actual materials
    /// </summary>
    private void Start()
    {
        materials = InstantiateMaterials(renderers);
    }

    /// <summary>
    /// Every frame, updates the parameter of each Material in the materials array
    /// </summary>
    private void LateUpdate()
    {
        foreach (Material material in materials)
            material.SetFloat(parameterName, parameterValue);
    }
}