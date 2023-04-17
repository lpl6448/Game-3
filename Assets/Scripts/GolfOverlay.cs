using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GolfOverlay : MonoBehaviour
{
    // Text containing the par information
    [SerializeField]
    private TextMeshProUGUI parText;

    // Text containing the stroke count
    [SerializeField]
    private TextMeshProUGUI strokeCount;

    /// <summary>
    /// Updates the par GUI to display the given par
    /// </summary>
    /// <param name="par">Par of the current level</param>
    public void UpdatePar(int par)
    {
        parText.text = "Par: " + par;
    }

    /// <summary>
    /// Updates the stroke GUI to display the given stroke count
    /// </summary>
    /// <param name="stroke">Current stroke count</param>
    public void UpdateStroke(int stroke)
    {
        strokeCount.text = "Strokes: " + stroke;
    }
}