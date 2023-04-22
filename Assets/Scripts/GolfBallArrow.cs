using UnityEngine;

public class GolfBallArrow : MonoBehaviour
{
    [SerializeField]
    private Transform line;

    [SerializeField]
    private float minLineWidth = 0.1f;

    [SerializeField]
    private float maxLineWidth = 0.25f;

    [SerializeField]
    private Material[] lineDashMaterials;

    [SerializeField]
    private Transform tip;

    [SerializeField]
    private Vector2 maxTipSize = new Vector2(0.5f, 0.25f);

    [SerializeField]
    private float arrowScaleExp = 1;

    public Vector3 Offset;

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