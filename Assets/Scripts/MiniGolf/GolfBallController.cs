using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages events for the ball and notifies other systems through UnityEvents when the ball is launched,
/// comes to a stop, goes out of bounds, etc.
/// Launch() is called from GolfPuttingInput
/// </summary>
public class GolfBallController : MonoBehaviour
{
    // GolfBallPhysics of this ball, used to check water coverage before resting
    [SerializeField]
    private GolfBallPhysics ballPhysics;

    // Event callback that runs whenever the ball is launched
    [SerializeField]
    private UnityEvent onPutt;

    // Event callback that runs whenever the ball comes to rest after putting
    [SerializeField]
    private UnityEvent onRest;

    // Event callback that runs whenever the ball goes into the hole.
    // NOTE: This can be called multiple times for a given level if the ball bounces, so it
    // should be ignored sometimes.
    [SerializeField]
    private UnityEvent onHole;

    // Event callback that runs every frame that the ball is out of bounds or submerged in water
    [SerializeField]
    private UnityEvent<RespawnReason> onOutOfBounds;

    // Minimum velocity for the ball to be still "moving"
    [SerializeField]
    private float velocityEpsilon;

    // Amount of time that the velocity should be under the velocityEpsilon before the ball is at rest
    [SerializeField]
    private float velocityTime;

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

    // Whether the ball was touching a level's bounds last frame
    // Level bounds are definod by Colliders (with trigger set to true) that have the tag of LevelBounds
    private bool isTouchingBounds = true;

    // Whether this ball has gone into the hole and triggered the celebration yet
    private bool triggeredCelebration = false;

    /// <summary>
    /// Called whenever the ball collides with a trigger. Check if the trigger has the LevelEnd tag and
    /// call the UnityEvent to end the level if so. If the trigger has the LevelOutOfBounds tag, instantly
    /// tell the ball that it has gone out of bounds (used to designate areas where the ball can never be).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelEnd")
        {
            // CELEBRATE!
            if (!triggeredCelebration)
            {
                other.GetComponentInParent<GolfHoleData>().PlayCelebration();
                triggeredCelebration = true;
            }

            onHole.Invoke();
        }
        else if (other.tag == "LevelOutOfBounds")
            onOutOfBounds.Invoke(RespawnReason.OutOfBounds);
    }

    /// <summary>
    /// Called whenever the ball continues to collide with a trigger. If the trigger has the LevelBounds tag,
    /// then the ball is still in bounds this frame.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "LevelBounds")
            isTouchingBounds = true;
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

        // Add torque to the ball to make it start spinning
        Vector3 torqueDir = Vector3.Cross(Vector3.up, velocity).normalized;
        rigidbody.AddTorque(torqueDir * velocity.magnitude * 600, ForceMode.VelocityChange);

        atRest = false;
        lastLaunchTime = Time.time;
        onPutt.Invoke();
    }

    /// <summary>
    /// Makes the ball "at rest," freezing it and calling the onRest events.
    /// If the ball's current position is not a valid rest position, it is actually out of bounds
    /// </summary>
    public void Rest()
    {
        if (IsValidRestPosition())
        {
            atRest = true;
            rigidbody.isKinematic = true;
            onRest.Invoke();
        }
        else
            onOutOfBounds.Invoke(RespawnReason.InvalidRestPosition);
    }

    /// <summary>
    /// Checks whether the ball's current position is a valid rest position or not.
    /// Currently any position is valid as long as the ball is not over half-submerged in water
    /// </summary>
    /// <returns></returns>
    private bool IsValidRestPosition()
    {
        // If the ball is currently over half-submerged in water, this is an invalid rest position
        return ballPhysics.WaterCoverage < 0.5f;
    }

    private void Start()
    {
        rigidbody.isKinematic = atRest;
        lastLaunchTime = Time.time;
    }

    private void FixedUpdate()
    {
        // If the ball is not touching a level's bounds, it is out of bounds and must be respawned
        // This still runs if the current level has no bounds, but the GolfGameManager should check the HasBounds flag of the level
        if (!isTouchingBounds)
            onOutOfBounds.Invoke(RespawnReason.OutOfBounds);
        isTouchingBounds = false; // Reset for next frame

        if (rigidbody.velocity.sqrMagnitude > velocityEpsilon * velocityEpsilon)
            lastVelocityTime = Time.time;

        // If the ball is fully submerged in water, it is out of bounds
        if (ballPhysics.WaterCoverage >= 0.99f)
            onOutOfBounds.Invoke(RespawnReason.Water);

        // If the ball is not at rest and has been roughly still for one second, make it at rest
        if (!atRest && Time.time - lastVelocityTime > velocityTime && Time.time - lastLaunchTime > 0.5f)
            Rest();
    }
}