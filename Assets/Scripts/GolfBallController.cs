using UnityEditor.U2D;
using UnityEngine;

/// <summary>
/// Controls basic physics for the golf ball, like angular damping and launching the ball.
/// Launch() is called from GolfPuttingInput
/// </summary>
public class GolfBallController : MonoBehaviour
{
    /// <summary>
    /// Degrees per second of angular "friction" applied to the ball to stop its angular momentum
    /// </summary>
    [SerializeField]
    private float angularDamping;

    /// <summary>
    /// Rigidbody of the golf ball
    /// </summary>
    [SerializeField]
    private new Rigidbody rigidbody;

    /// <summary>
    /// Every physics tick that this ball is colliding with the ground, apply the angular damping
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * Mathf.Min(angularDamping * Time.fixedDeltaTime, rigidbody.angularVelocity.magnitude);
    }

    /// <summary>
    /// Applies an impulse to this ball
    /// </summary>
    /// <param name="velocity">Velocity to add</param>
    public void Launch(Vector3 velocity)
    {
        // Add an impulse to the ball
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }
}