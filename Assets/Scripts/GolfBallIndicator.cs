using UnityEngine;

/// <summary>
/// Controls the golf ball's putting indicator (the line that appears underneath it whenever the ball is dragged.
/// DragOffset is set inside the GolfPuttingInput script.
/// </summary>
public class GolfBallIndicator : MonoBehaviour
{
    /// <summary>
    /// Transform of the golf ball that the indicator's origin is always under
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// Line Transform that reflects the drag indicator's distance and direction
    /// </summary>
    [SerializeField]
    private Transform line;
    
    /// <summary>
    /// Units in the y-direction to displace the line below the target Transform
    /// </summary>
    [SerializeField]
    private float verticalOffset;

    /// <summary>
    /// Animation component that is played whenever the stroke is over and the user can putt the ball again
    /// </summary>
    [SerializeField]
    private Animation highlightAnimation;

    /// <summary>
    /// World-space offset applied to the line Transform
    /// </summary>
    public Vector3 DragOffset { get; set; }

    /// <summary>
    /// Every frame, move this indicator under the target Transform and make the line point from
    /// right under the ball to the position indicated by the DragOffset
    /// </summary>
    private void LateUpdate()
    {
        transform.position = target.position + Vector3.up * verticalOffset;

        if (DragOffset.sqrMagnitude > 0.001f)
        {
            line.localPosition = DragOffset / 2;
            line.localRotation = Quaternion.LookRotation(DragOffset.normalized) * Quaternion.Euler(90, 0, 0);
        }
        line.localScale = new Vector3(line.localScale.x, DragOffset.magnitude, line.localScale.z);
    }

    /// <summary>
    /// Plays the highlight animation effect (when the user can putt the ball again)
    /// </summary>
    public void PlayHighlightAnimation()
    {
        highlightAnimation.Play();
    }
}