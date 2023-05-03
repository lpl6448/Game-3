using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float rotationLerpFactor = 6; // Controls the camera rotation smoothing

    [SerializeField]
    private RectTransform chevronLeft; // Reference to the chevron on the left that moves down when the camera is moving
    [SerializeField]
    private RectTransform chevronRight; // Reference to the chevron on the left that moves down when the camera is moving
    [SerializeField]
    private float hoverChevronOffset; // Number of canvas units to move the indicator chevrons when the camera is moving

    private float currentRotation = 0; // Current y-rotation of the camera
    private float goalRotation = 0; // Target y-rotation of the camera

    void Start()
    {
        GameData.gameState = State.Hub;
    }

    // Update is called once per frame
    void Update()
    {
        float targetLeftOffset = 0;
        float targetRightOffset = 0;

        //Stop camera controls if not in hub state
        if (GameData.gameState == State.Hub)
        {
            if (Input.mousePosition.x > Screen.width - Screen.height / 5)
            {
                LookRight();
                targetRightOffset = hoverChevronOffset;
            }
            else if (Input.mousePosition.x < Screen.height / 5)
            {
                LookLeft();
                targetLeftOffset = hoverChevronOffset;
            }
        }

        // Smooth and update the chevron positions
        chevronLeft.anchoredPosition = new Vector2(chevronLeft.anchoredPosition.x,
            Mathf.Lerp(chevronLeft.anchoredPosition.y, targetLeftOffset, 1 - Mathf.Exp(-5 * Time.deltaTime)));
        chevronRight.anchoredPosition = new Vector2(chevronRight.anchoredPosition.x,
            Mathf.Lerp(chevronRight.anchoredPosition.y, targetRightOffset, 1 - Mathf.Exp(-5 * Time.deltaTime)));

        // Smooth and update the camera's rotation
        currentRotation = Mathf.Lerp(currentRotation, goalRotation, 1 - Mathf.Exp(-rotationLerpFactor * Time.deltaTime));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentRotation, transform.eulerAngles.z);
    }

    /// <summary>
    /// Shift camera right when called
    /// </summary>
    public void LookRight()
    {
        if (transform.rotation.y < 0.3420216f)
            goalRotation += rotSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Shift camera left when called
    /// </summary>
    public void LookLeft()
    {
        if (transform.rotation.y > -0.3420201f)
            goalRotation -= rotSpeed * Time.deltaTime;
    }
}
