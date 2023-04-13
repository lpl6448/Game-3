using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls basic physics for the golf ball, like angular damping and launching the ball.
/// Launch() is called from GolfPuttingInput
/// </summary>
public class GolfBallController : MonoBehaviour
{
    // Event callback that runs whenever the ball is launched
    [SerializeField]
    private UnityEvent onPutt;

    // Event callback that runs whenever the ball comes to rest after putting
    [SerializeField]
    private UnityEvent onRest;

    /// <summary>
    /// Degrees per second of angular "friction" applied to the ball to stop its angular momentum
    /// </summary>
    [SerializeField]
    private float angularDamping;

    // Minimum velocity for the ball to be still "moving"
    [SerializeField]
    private float velocityEpsilon;

    /// <summary>
    /// Rigidbody of the golf ball
    /// </summary>
    [SerializeField]
    private new Rigidbody rigidbody;

    // Whether the ball is currently resting / not moving
    private bool atRest = false;

    // Time.time when the ball was last putted
    private float lastLaunchTime;

    // Time.time when the ball last had a velocity greater than an epsilon value
    private float lastVelocityTime;

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
        // Unfreeze and add an impulse to the ball
        rigidbody.isKinematic = false;
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);

        atRest = false;
        lastLaunchTime = Time.time;
        onPutt.Invoke();
    }

    private void Start()
    {
        rigidbody.isKinematic = atRest;
        lastLaunchTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (rigidbody.velocity.sqrMagnitude > velocityEpsilon * velocityEpsilon)
            lastVelocityTime = Time.time;

        // If the ball is not at rest and has been roughly still for one second, make it at rest
        if (!atRest && Time.time - lastVelocityTime > 1 && Time.time - lastLaunchTime > 1)
        {
            atRest = true;
            rigidbody.isKinematic = true;
            onRest.Invoke();
        }
    }
}