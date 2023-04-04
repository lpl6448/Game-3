using UnityEngine;

public class GolfCameraController : MonoBehaviour
{
    public float CameraSensitivity;

    public float TargetDistance;

    public Transform Target;

    /// <summary>
    /// Whether the mouse is currently locked.
    /// </summary>
    private bool lockMouse = false;

    /// <summary>
    /// Current Euler angles of the camera
    /// </summary>
    private Vector3 cameraAngles;

    /// <summary>
    /// To initialize, set the cameraAngles to face the Target transform
    /// </summary>
    private void Start()
    {
        cameraAngles = Quaternion.LookRotation(Target.position - transform.position).eulerAngles;
    }

    private void Update()
    {
        // First, update whether the mouse is locked based on whether the user is holding down the right mouse button (for now)
        if (Input.GetMouseButton(1))
        {
            lockMouse = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            lockMouse = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // If the mouse is locked, move the camera based on mouse movement
        if (lockMouse)
        {
            Vector3 lookInput = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
            Vector3 angleDelta = lookInput * CameraSensitivity;
            cameraAngles += angleDelta;

            // Prevent the camera from turning upside-down
            cameraAngles.x = Mathf.Clamp(cameraAngles.x, -90, 90);
        }

        // Update the camera's rotation and position
        transform.eulerAngles = cameraAngles;
        transform.position = Target.position - transform.forward * TargetDistance;
    }
}