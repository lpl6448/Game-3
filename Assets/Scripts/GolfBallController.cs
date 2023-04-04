using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public float PuttSpeed;

    public float VerticalComponent;

    public float AngularDamping;

    [SerializeField]
    private new Rigidbody rigidbody;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Figure out putt direction from camera
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 puttDir = new Vector3(camForward.x, 0, camForward.z).normalized;

            // Add an impulse to the ball
            rigidbody.AddForce(puttDir * PuttSpeed, ForceMode.VelocityChange);
            rigidbody.AddForce(Vector3.up * VerticalComponent * PuttSpeed, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * Mathf.Min(AngularDamping * Time.fixedDeltaTime, rigidbody.angularVelocity.magnitude);
    }
}