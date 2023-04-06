using UnityEngine;

public class GolfBallIndicator : MonoBehaviour
{
    public Transform Target;
    public Transform ForwardReference;
    public Transform Line;

    public float VerticalOffset;
    public Vector3 DragOffset;

    private void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * VerticalOffset;
        transform.forward = new Vector3(ForwardReference.forward.x, 0, ForwardReference.forward.z).normalized;

        if (DragOffset.sqrMagnitude > 0.001f)
        {
            Line.position = transform.position + DragOffset / 2;
            Line.rotation = Quaternion.LookRotation(DragOffset.normalized) * Quaternion.Euler(90, 0, 0);
        }
        Line.localScale = new Vector3(Line.localScale.x, DragOffset.magnitude, Line.localScale.z);
    }
}