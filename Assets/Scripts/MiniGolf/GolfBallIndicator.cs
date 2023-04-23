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
    public Transform Target;

    /// <summary>
    /// Line Transform that reflects the drag indicator's distance and direction
    /// </summary>
    [SerializeField]
    private Transform line;

    /// <summary>
    /// Arrow that reflects the direction and velocity that the ball will launch with
    /// </summary>
    [SerializeField]
    private GolfBallArrow arrow;

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

    // TrailRenderer to enable based on whether input is active or not
    [SerializeField]
    private TrailRenderer trailRenderer;

    /// <summary>
    /// World-space offset applied to the line Transform
    /// </summary>
    public Vector3 DragOffset { get; set; }

    /// <summary>
    /// World-space offset applied to the arrow
    /// </summary>
    public Vector3 ArrowOffset { get; set; }

    /// <summary>
    /// Move the indicator to the ball before the first frame
    /// </summary>
    private void Awake()
    {
        SetTrails(false);
        if (Target != null)
            transform.position = Target.position + Vector3.up * verticalOffset;
    }

    /// <summary>
    /// Every frame, move this indicator under the target Transform and make the line point from
    /// right under the ball to the position indicated by the DragOffset
    /// </summary>
    private void LateUpdate()
    {
        if (Target != null)
            transform.position = Target.position + Vector3.up * verticalOffset;

        if (DragOffset.sqrMagnitude > 0.001f)
        {
            line.localPosition = DragOffset / 2;
            line.localRotation = Quaternion.LookRotation(DragOffset.normalized) * Quaternion.Euler(90, 0, 0);
        }
        line.localScale = new Vector3(line.localScale.x, DragOffset.magnitude, line.localScale.z);

        arrow.Offset = ArrowOffset;
    }

    /// <summary>
    /// Plays the highlight animation effect (when the user can putt the ball again)
    /// </summary>
    public void PlayHighlightAnimation()
    {
        highlightAnimation.Play();
    }

    public void SetTrails(bool emitting)
    {
        trailRenderer.emitting = emitting;
    }
}