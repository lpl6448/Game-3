using UnityEngine;

/// <summary>
/// Controls the Camera in the mini golf game mode, rotating the camera and gradually
/// following the ball. StartDrag() and EndDrag() are called from GolfPuttingInput
/// </summary>
public class GolfCameraController : MonoBehaviour
{
    /// <summary>
    /// Mouse sensitivity (x: horizontal, y: vertical) of the camera movement
    /// </summary>
    [SerializeField]
    private Vector2 cameraMouseSensitivity = new Vector2(1, 1);

    /// <summary>
    /// Minimum camera pitch in degrees (farthest up that the player can drag the camera)
    /// </summary>
    [SerializeField]
    [Range(-90, 90)]
    private float minCameraPitch = -90;

    /// <summary>
    /// Maximum camera pitch in degrees (farthest down that the player can drag the camera)
    /// </summary>
    [SerializeField]
    [Range(-90, 90)]
    private float maxCameraPitch = 90;

    /// <summary>
    /// Interpolation exponential factor used to smooth out mouse movements
    /// (I made this factor somewhat arbitrary, but higher numbers mean faster/rougher movement)
    /// </summary>
    [SerializeField]
    private float cameraLookLerp = 8;

    /// <summary>
    /// Target Transform that the camera will follow
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// Distance from the target that this camera orbits at
    /// </summary>
    [SerializeField]
    private float targetDistance = 8;

    /// <summary>
    /// Interpolation exponential factor used to smooth out the camera moving to follow the target
    /// (I made this factor somewhat arbitrary, but higher numbers mean faster/rougher movement)
    /// </summary>
    [SerializeField]
    private float cameraFollowLerp = 8;

    /// <summary>
    /// Rotation that the camera is moving toward
    /// </summary>
    private Vector3 goalCameraAngles;

    /// <summary>
    /// Current target position that the camera is following (used for smoothing)
    /// </summary>
    private Vector3 currentTargetPosition;

    /// <summary>
    /// Whether the camera is currently being dragged or not
    /// </summary>
    private bool dragging = false;

    /// <summary>
    /// Direction pointed to the camera's right, with a y-value of 0
    /// </summary>
    public Vector3 FlatRight => new Vector3(transform.right.x, 0, transform.right.z).normalized;

    /// <summary>
    /// Direction pointed in the camera's forward direction, with a y-value of 0
    /// </summary>
    public Vector3 FlatForward => new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

    /// <summary>
    /// Begins to drag this camera using mouse movements
    /// </summary>
    public void StartDrag()
    {
        dragging = true;
    }

    /// <summary>
    /// Stops dragging this camera using mouse movements
    /// </summary>
    public void EndDrag()
    {
        dragging = false;
    }

    /// <summary>
    /// To initialize, set the cameraAngles to face the Target transform
    /// </summary>
    private void Start()
    {
        currentTargetPosition = target.position;
        goalCameraAngles = Quaternion.LookRotation(target.position - transform.position).eulerAngles;
    }

    private void Update()
    {
        // First, update whether the mouse is locked
        if (dragging)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // If the mouse is locked, move the camera based on mouse movement
        Vector3 lookInput = Vector3.zero;
        if (dragging)
        {
            lookInput = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
        }

        // Rotate the camera angles
        Vector3 angleDelta = Vector3.Scale(lookInput, cameraMouseSensitivity);
        goalCameraAngles += angleDelta;

        // Enforce min/max camera pitch rules
        goalCameraAngles.x = Mathf.Clamp(goalCameraAngles.x, minCameraPitch, maxCameraPitch);

        // Update the camera's rotation and position
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalCameraAngles), 1 - Mathf.Exp(-cameraLookLerp * Time.deltaTime));
        currentTargetPosition = Vector3.Lerp(currentTargetPosition, target.position, 1 - Mathf.Exp(-cameraFollowLerp * Time.deltaTime));
        transform.position = currentTargetPosition - transform.forward * targetDistance;
    }
}