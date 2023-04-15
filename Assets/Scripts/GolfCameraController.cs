using UnityEngine;

/// <summary>
/// Controls the Camera in the mini golf game mode, rotating the camera and gradually
/// following the ball. StartDrag() and EndDrag() are called from GolfPuttingInput
/// </summary>
public class GolfCameraController : MonoBehaviour
{
    /// <summary>
    /// If the camera is frozen (like when the ball goes out of bounds), it stops following accepting mouse drag input
    /// and stops following the target
    /// </summary>
    public bool Frozen = false;

    /// <summary>
    /// Mouse sensitivity (x: horizontal, y: vertical) of the camera movement
    /// </summary>
    [SerializeField]
    private Vector2 cameraMouseSensitivity = new Vector2(1, 1);

    /// <summary>
    /// Sensitivity of the scroll wheel used to zoom the camera in/out
    /// </summary>
    [SerializeField]
    private float cameraZoomSensitivity = 10;

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
    /// Interpolation exponential factor used to smooth out the zoom level of the camera
    /// </summary>
    [SerializeField]
    private float cameraZoomLerp = 6;

    /// <summary>
    /// Target Transform that the camera will follow
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// Minimum distance from the target that this camera orbits at
    /// </summary>
    [SerializeField]
    private float minTargetDistance = 2;

    /// <summary>
    /// Maximum distance from the target that this camera orbits at
    /// </summary>
    [SerializeField]
    private float maxTargetDistance = 16;

    /// <summary>
    /// Distance from the target that the camera will aim to zoom to
    /// </summary>
    [SerializeField]
    private float goalTargetDistance = 8;

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
    /// Current log distance from the target that the camera orbits at
    /// </summary>
    private float currentTargetDistanceLog;

    /// <summary>
    /// Current target position that the camera is following (used for smoothing)
    /// </summary>
    private Vector3 currentTargetPosition;

    /// <summary>
    /// Target position that the camera will aim to follow/orbit
    /// </summary>
    private Vector3 goalTargetPosition;

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
        if (!Frozen)
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
        goalTargetPosition = target.position;
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

        if (!Frozen)
        {
            // If the mouse is locked, move the camera based on mouse movement
            Vector3 lookInput = Vector3.zero;
            if (dragging && !Frozen)
            {
                lookInput = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
            }

            // Rotate the camera angles
            Vector3 angleDelta = Vector3.Scale(lookInput, cameraMouseSensitivity);
            goalCameraAngles += angleDelta;

            // Enforce min/max camera pitch rules
            goalCameraAngles.x = Mathf.Clamp(goalCameraAngles.x, minCameraPitch, maxCameraPitch);

            // Zoom the camera in or out
            float goalTargetDistanceLog = Mathf.Log(goalTargetDistance);
            goalTargetDistanceLog -= Input.GetAxisRaw("Mouse ScrollWheel") * cameraZoomSensitivity;
            goalTargetDistance = Mathf.Clamp(Mathf.Exp(goalTargetDistanceLog), minTargetDistance, maxTargetDistance);

            goalTargetPosition = target.position;
        }

        // Update the camera's rotation and position
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalCameraAngles), 1 - Mathf.Exp(-cameraLookLerp * Time.deltaTime));
        currentTargetPosition = Vector3.Lerp(currentTargetPosition, goalTargetPosition, 1 - Mathf.Exp(-cameraFollowLerp * Time.deltaTime));
        currentTargetDistanceLog = Mathf.Lerp(currentTargetDistanceLog, Mathf.Log(goalTargetDistance), 1 - Mathf.Exp(-cameraZoomLerp * Time.deltaTime));
        transform.position = currentTargetPosition - transform.forward * Mathf.Exp(currentTargetDistanceLog);
    }
}