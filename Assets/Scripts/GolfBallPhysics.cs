using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls basic physics for the golf ball, like angular damping and launching the ball.
/// Launch() is called from GolfPuttingInput
/// </summary>
public class GolfBallPhysics : MonoBehaviour
{
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

    /// <summary>
    /// Degrees per second of angular "friction" applied to the ball to stop its angular momentum
    /// </summary>
    [SerializeField]
    private float angularDamping;

    // Maximum drag to apply to the ball when it is fully submerged
    [SerializeField]
    private float waterDrag;

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

    // Maximum amount of water covering the ball this physics update (0 - not submerged, 1 - fully submerged)
    private float waterCoverage = 0;

    /// <summary>
    /// Every physics tick that this ball is colliding with the ground, apply the angular damping
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        rigidbody.angularVelocity -= rigidbody.angularVelocity.normalized * Mathf.Min(angularDamping * Time.fixedDeltaTime, rigidbody.angularVelocity.magnitude);
    }

    /// <summary>
    /// Called whenever the ball collides with a trigger. Check if the trigger has the LevelEnd tag and
    /// call the UnityEvent to end the level if so.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelEnd")
            onHole.Invoke();
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
        else if (other.tag == "WaterCollider")
        {
            // Raycast downward to approximate how much water the ball is in
            if (other.Raycast(new Ray(transform.position + Vector3.up * 1000, Vector3.down), out RaycastHit hit, 10000))
            {
                float newWaterCoverage = Mathf.Clamp01(hit.point.y - transform.position.y + 0.5f);
                waterCoverage = Mathf.Max(waterCoverage, newWaterCoverage);
            }
        }
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
        return waterCoverage < 0.5f;
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

        // Apply drag to the rigidbody based on the amount of water coverage this frame
        if (waterCoverage > 0)
        {
            // Calculate the volume of this ball that is submerged in the water. (Technically drag doesn't work this way, but it should be close enough.)
            float b = waterCoverage * 2 - 1;
            float waterSubmersion = -(b - 2) * (b + 1) * (b + 1) / 3;
            rigidbody.velocity *= Mathf.Clamp01(1 - rigidbody.velocity.magnitude * waterSubmersion * waterDrag * Time.fixedDeltaTime);

            // If the ball is fully submerged in water, it is out of bounds
            if (waterCoverage >= 0.99f)
                onOutOfBounds.Invoke(RespawnReason.Water);
        }

        if (rigidbody.velocity.sqrMagnitude > velocityEpsilon * velocityEpsilon)
            lastVelocityTime = Time.time;

        // If the ball is not at rest and has been roughly still for one second, make it at rest
        if (!atRest && Time.time - lastVelocityTime > velocityTime && Time.time - lastLaunchTime > 0.5f)
            Rest();

        // Reset water coverage for next frame
        waterCoverage = 0;
    }
}