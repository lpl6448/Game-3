using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public float PuttSpeed;

    public float VerticalComponent;

    public float AngularDamping;

    [SerializeField]
    private new Rigidbody rigidbody;

    [SerializeField]
    private GolfBallIndicator indicator;

    private bool dragging = false;

    private Vector3 dragOrigin;

    public void StartDrag()
    {
        dragging = true;
        dragOrigin = Vector3.zero;
        dragOrigin = GetDragOffset();
    }
    public void EndDrag()
    {
        dragging = false;

        Launch(-GetDragOffset() * PuttSpeed);
        indicator.DragOffset = Vector3.zero;
    }

    private void Update()
    {
        if (dragging)
            indicator.DragOffset = GetDragOffset();
    }

    private void OnCollisionStay(Collision collision)
    {
        rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * Mathf.Min(AngularDamping * Time.fixedDeltaTime, rigidbody.angularVelocity.magnitude);
    }

    private Vector3 GetDragOffset()
    {
        Plane dragPlane = new Plane(Vector3.up, dragOrigin);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(mouseRay, out float t))
        {
            Vector3 worldPoint = mouseRay.GetPoint(t);
            return worldPoint - dragOrigin;
        }
        return dragOrigin;
    }

    private void Launch(Vector3 velocity)
    {
        // Add an impulse to the ball
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        rigidbody.AddForce(Vector3.up * VerticalComponent * velocity.magnitude, ForceMode.VelocityChange);
    }
}