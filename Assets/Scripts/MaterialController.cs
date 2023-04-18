using System;
using UnityEngine;

/// <summary>
/// Utility component that just updates a list of materials each frame with a particular parameter
/// </summary>
public class MaterialController : MonoBehaviour
{
    // Parameter name in the Material that is changed
    [SerializeField]
    private string parameterName;

    // Current parameter value in the Material
    [SerializeField]
    private float parameterValue;

    // Array of all Materials to update every frame
    [SerializeField]
    private Material[] materials;

    /// <summary>
    /// Every frame, updates the parameter of each Material in the materials array
    /// </summary>
    private void LateUpdate()
    {
        foreach (Material material in materials)
                material.SetFloat(parameterName, parameterValue);
    }
}