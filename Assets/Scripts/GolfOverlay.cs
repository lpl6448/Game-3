using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GolfOverlay : MonoBehaviour
{
    // Text containing the par information
    [SerializeField]
    private TextMeshProUGUI parText;

    // Text containing the stroke count
    [SerializeField]
    private TextMeshProUGUI strokeCount;

    // Reference to the Animator controlling in/out animations
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// Transitions the overlay in or out depending on whether it should be visible
    /// </summary>
    /// <param name="visible">Whether the overlay should be visible on-screen</param>
    /// <param name="delay">Number of seconds that will pass before applying the new visibility</param>
    public void UpdateVisibility(bool visible, float delay = 0)
    {
        if (delay <= 0)
            animator.SetBool("In", visible);
        else
            StartCoroutine(WaitToUpdateVisibility(visible, delay));
    }

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

    /// <summary>
    /// Waits the given number of seconds before animating the overlay in/out
    /// </summary>
    /// <param name="visible">Whether the overlay should be visible</param>
    /// <param name="delay">Number of seconds to wait before applying the change</param>
    /// <returns></returns>
    private IEnumerator WaitToUpdateVisibility(bool visible, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("In", visible);
    }
}