using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls basic physics for the golf ball, like angular damping and water drag.
/// </summary>
public class GolfBallPhysics : MonoBehaviour
{
    /// <summary>
    /// Degrees per second of angular "friction" applied to the ball to stop its angular momentum
    /// </summary>
    [SerializeField]
    private float angularDamping;

    // Maximum drag to apply to the ball when it is fully submerged
    [SerializeField]
    private float waterDrag;

    /// <summary>
    /// Rigidbody of the golf ball
    /// </summary>
    [SerializeField]
    private new Rigidbody rigidbody;

    // Maximum amount of water covering the ball this physics update (0 - not submerged, 1 - fully submerged)
    public float WaterCoverage { get; private set; }

    /// <summary>
    /// Every physics tick that this ball is colliding with the ground, apply the angular damping
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * Mathf.Min(angularDamping * Time.fixedDeltaTime, rigidbody.angularVelocity.magnitude);
    }

    /// <summary>
    /// Called whenever the ball continues to collide with a trigger. If the trigger has the WaterCollider tag, then it is
    /// partially/fully submerged in water this frame.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "WaterCollider")
        {
            // Raycast downward to approximate how much water the ball is in
            if (other.Raycast(new Ray(transform.position + Vector3.up * 1000, Vector3.down), out RaycastHit hit, 10000))
            {
                float newWaterCoverage = Mathf.Clamp01(hit.point.y - transform.position.y + 0.5f);
                WaterCoverage = Mathf.Max(WaterCoverage, newWaterCoverage);
            }
        }
    }

    private void FixedUpdate()
    {
        // Apply drag to the rigidbody based on the amount of water coverage this frame
        if (WaterCoverage > 0)
        {
            // Calculate the volume of this ball that is submerged in the water. (Technically drag doesn't work this way, but it should be close enough.)
            float b = WaterCoverage * 2 - 1;
            float waterSubmersion = -(b - 2) * (b + 1) * (b + 1) / 3;
            rigidbody.velocity *= Mathf.Clamp01(1 - rigidbody.velocity.magnitude * waterSubmersion * waterDrag * Time.fixedDeltaTime);
        }

        // Reset water coverage for next frame
        WaterCoverage = 0;
    }
}