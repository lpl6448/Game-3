using UnityEngine;

/// <summary>
/// Class that controls the arrow indicator, which shows the direction/speed the ball will be putt in
/// </summary>
public class GolfBallArrow : MonoBehaviour
{
    // Transform of the dotted line part of the arrow
    [SerializeField]
    private Transform line;

    // Minimum line width, when the putt speed is 0
    [SerializeField]
    private float minLineWidth = 0.1f;

    // Maximum line width, as the putt speed approaches infinity
    [SerializeField]
    private float maxLineWidth = 0.25f;

    // Array of renderers attached to lines (for dotted line effect)
    [SerializeField]
    private Renderer[] lineDashRenderers;

    // Transform of the arrow's tip object
    [SerializeField]
    private Transform tip;

    // Maximum scale of the tip (x=horiziontal, y=vertical) as the putt speed approaches infinity
    [SerializeField]
    private Vector2 maxTipSize = new Vector2(0.5f, 0.25f);

    // Exponential factor that determines how fast the maximum line width and tip size are reached
    [SerializeField]
    private float arrowScaleExp = 1;

    // Internal array of Materials that get updated with the dotted line effect
    private Material[] lineDashMaterials;

    // 3D Offset, set by GolfBallIndicator, of the arrow tip from this object's world position
    public Vector3 Offset;

    /// <summary>
    /// To initialize, create new temporary materials that can be changed without affecting the originals
    /// </summary>
    private void Start()
    {
        lineDashMaterials = MaterialController.InstantiateMaterials(lineDashRenderers);
    }

    /// <summary>
    /// Every frame, update the line and tip Transforms' position, scale, and rotation to reflect the Offset
    /// and update lineDashMaterials so that the line's dotted nature does not stretch.
    /// </summary>
    private void LateUpdate()
    {
        float offsetLength = Offset.magnitude;
        if (offsetLength > 0.01f)
        {
            Vector3 offsetDir = Offset / offsetLength;

            float scaleMultiplier = 1 - Mathf.Exp(-offsetLength * arrowScaleExp);
            float lineWidth = Mathf.Max(minLineWidth, maxLineWidth * scaleMultiplier);
            Vector2 tipSize = maxTipSize * scaleMultiplier;
            float lineLength = Mathf.Max(0, offsetLength - tipSize.y);

            line.position = transform.position + offsetDir * lineLength / 2;
            line.localScale = new Vector3(lineWidth, lineLength, 1);
            line.rotation = Quaternion.LookRotation(Offset.normalized) * Quaternion.Euler(90, 180, 0);

            tip.position = transform.position + offsetDir * (offsetLength - tipSize.y / 2);
            tip.localScale = new Vector3(tipSize.x, tipSize.y, 1);
            tip.rotation = Quaternion.LookRotation(Offset.normalized) * Quaternion.Euler(90, 0, 0);

            foreach (Material mat in lineDashMaterials)
                mat.mainTextureScale = new Vector2(1, lineLength / lineWidth / 3);
        }
        else
        {
            line.localScale = Vector3.zero;
            tip.localScale = Vector3.zero;
        }
    }
}