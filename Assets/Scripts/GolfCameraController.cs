using UnityEngine;

public class GolfCameraController : MonoBehaviour
{
    public Vector2 CameraMouseSensitivity;

    public float KeyboardSensitivity;

    public float CameraLookLerp;

    public float TargetDistance;

    public Transform Target;

    private Vector3 goalCameraAngles;

    private bool dragging = false;

    public void StartDrag()
    {
        dragging = true;
    }
    public void EndDrag()
    {
        dragging = false;
    }

    /// <summary>
    /// To initialize, set the cameraAngles to face the Target transform
    /// </summary>
    private void Start()
    {
        goalCameraAngles = Quaternion.LookRotation(Target.position - transform.position).eulerAngles;
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

        // Keyboard camera controls
        lookInput.y -= Input.GetAxisRaw("Horizontal") * KeyboardSensitivity * Time.deltaTime;

        // Rotate the camera angles
        Vector3 angleDelta = Vector3.Scale(lookInput, CameraMouseSensitivity);
        goalCameraAngles += angleDelta;

        // Prevent the camera from turning upside-down
        goalCameraAngles.x = Mathf.Clamp(goalCameraAngles.x, -90, 90);

        // Update the camera's rotation and position
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalCameraAngles), 1 - Mathf.Pow(1 - CameraLookLerp, Time.deltaTime));
        transform.position = Target.position - transform.forward * TargetDistance;
    }
}