using UnityEngine;

/// <summary>
/// Central control class that lets the player putt the ball and controls displays like the indicator.
/// </summary>
public class GolfPuttingInput : MonoBehaviour
{
    // Putt speed when the indicator is exactly 1 unit from the ball.
    // At other distances, the putt speed scales proportionally to the square of the distance (for now).
    [SerializeField]
    private float referencePuttSpeed;

    [SerializeField]
    private float minimumPuttSpeed;

    // Vertical amount - which is the result sin(angle) - to launch the ball up into the air
    [SerializeField]
    private float verticalComponent;

    // The max distance that the putter (or indicator) can be from the ball, imposed as a soft limit
    [SerializeField]
    private float maxPutterDistance = 4;

    // Exponential limit factor applied to the putter distance
    // (See ExponentialLimit() function at the bottom of this script.)
    [SerializeField]
    private float putterDistanceExpFactor = 1;

    [SerializeField]
    private AnimationCurve distanceResponseCurve;

    [SerializeField]
    private float arrowDistanceScale = 1;

    // Reference to the game manager used to check pause state
    [SerializeField]
    private GolfGameManager gameManager;

    // Reference to the camera controller for the mini golf game mode
    [SerializeField]
    private GolfCameraController cameraController;

    // Reference to the golf ball's indicator
    [SerializeField]
    private GolfBallIndicator indicator;

    // Reference to the golf ball itself, used to launch the ball
    [SerializeField]
    private GolfBallController ballController;

    // Reference to the overlay (controlling the visibility animation)
    [SerializeField]
    private GolfOverlay golfOverlay;

    // LayerMask (probably just containing the "Golf Ball" layer).
    // If the user drags anything in this LayerMask (probably only the golf ball), it drags the golf ball and putts it.
    [SerializeField]
    private LayerMask golfBallDragMask;

    // Whether the golf ball is currently being "dragged" and about to be launched
    private bool draggingBall = false;

    // Whether the camera is currently being dragged and rotated by mouse movements
    private bool draggingCamera = false;

    // Whether the ball can be dragged and launched
    public bool AllowInput = false;

    public void ActivateInput()
    {
        AllowInput = true;
        golfOverlay.UpdateVisibility(true, 0);
        indicator.PlayHighlightAnimation();
    }
    public void DeactivateInput() => AllowInput = false;

    /// <summary>
    /// Launches the ball with the calculated velocity (based on mouse input)
    /// and disables the golf overlay after a brief moment
    /// </summary>
    private void LaunchBall()
    {
        Vector3 velocity = GetLaunchVelocity();
        if (velocity.magnitude >= minimumPuttSpeed)
        {
            SFXHandler.Instance.playPutt();
            ballController.Launch(GetLaunchVelocity());
            golfOverlay.UpdateVisibility(false, 0);
        }
    }

    /// <summary>
    /// Every frame, if the user has just clicked, try to drag either the camera or the ball.
    /// When the user has let go of the mouse button, putt the ball or stop dragging the camera.
    /// </summary>
    private void Update()
    {
        // Cancel ball drag if the player presses escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            draggingBall = false;
            indicator.DragOffset = Vector3.zero;
            indicator.ArrowOffset = Vector3.zero;
        }

        if (!gameManager.Paused)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!draggingBall) // If the ball is not being dragged, start dragging the camera if the player right clicks
                {
                    draggingCamera = true;
                    cameraController.StartDrag();
                }
                else // If the ball is being dragged and the player right clicks, cancel the drag
                {
                    draggingBall = false;
                    indicator.DragOffset = Vector3.zero;
                    indicator.ArrowOffset = Vector3.zero;
                }
            }

            if (Input.GetMouseButtonDown(0) && !draggingCamera && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, 100000, golfBallDragMask))
                {
                    // If we click the ball and allow input, then begin dragging the ball.
                    if (AllowInput)
                        draggingBall = true;
                }
                else
                {
                    // Otherwise, drag the camera (handled by the CameraController)
                    draggingCamera = true;
                    cameraController.StartDrag();
                }
            }
            else if (!Input.GetMouseButton(0))
            {
                // Once the user has let go of left mouse, launch the ball if it was being dragged
                if (draggingBall)
                {
                    draggingBall = false;
                    indicator.DragOffset = Vector3.zero;
                    indicator.ArrowOffset = Vector3.zero;

                    // If input is allowed, launch the ball
                    if (AllowInput)
                        LaunchBall();
                }
                if (draggingCamera && !Input.GetMouseButton(1))
                {
                    draggingCamera = false;
                    cameraController.EndDrag();
                }
            }
        }
        else
        {
            if (draggingCamera)
            {
                draggingCamera = false;
                cameraController.EndDrag();
            }
        }

        // If dragging the ball, update the indicator to show which way the ball will go.
        if (draggingBall)
        {
            indicator.DragOffset = ModifyDragOffset();
            Vector3 launchVel = GetLaunchVelocity();
            indicator.ArrowOffset = new Vector3(launchVel.x, 0, launchVel.z) * arrowDistanceScale;
        }
    }

    /// <summary>
    /// Uses the modified drag offset to get a launch velocity for the golf ball.
    /// This velocity is scaled according to the square of the modified drag distance.
    /// Then, a vertical component is added to the velocity to launch the ball slightly upward.
    /// </summary>
    /// <returns>Launch velocity to apply to the golf ball</returns>
    private Vector3 GetLaunchVelocity()
    {
        Vector3 dragOffset = ModifyDragOffset();
        Vector3 launchDir = -dragOffset.normalized;
        Vector3 launchVel = launchDir * distanceResponseCurve.Evaluate(dragOffset.magnitude / maxPutterDistance);
        launchVel *= referencePuttSpeed;
        launchVel.y += launchVel.magnitude * verticalComponent;
        return launchVel;
    }

    /// <summary>
    /// Gets the drag offset, with the direction or length modified to better reflect the velocity of the ball when launched
    /// </summary>
    /// <returns>Vector3 where x and z correspond to a drag offset with soft limits applied</returns>
    private Vector3 ModifyDragOffset()
    {
        Vector3 dragOffset = GetRawDragOffset();
        float dragLength = dragOffset.magnitude;
        dragLength = ExponentialLimit(dragLength, maxPutterDistance, putterDistanceExpFactor);
        return dragOffset.normalized * dragLength;
    }

    /// <summary>
    /// Gets the world-space "raw" drag offset, which is the offset from the ball's position on the xy-plane
    /// </summary>
    /// <returns>Vector3, where x and z correspond to the offset from the ball position to the mouse position</returns>
    private Vector3 GetRawDragOffset()
    {
        Plane dragPlane = new Plane(Vector3.down, ballController.transform.position.y - 0.5f);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(mouseRay, out float t))
        {
            Vector3 worldPoint = mouseRay.GetPoint(t);
            Vector3 dragDelta = worldPoint - ballController.transform.position;
            dragDelta.y = 0;
            return dragDelta;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// The ModifyDragOffset() function uses a "soft limit" to make the indicator/ball velocity feel like a spring.
    /// As the user drags the mouse further back, the indicator moves less and less, giving the impression of a spring.
    /// This function imposes that "soft limit," where the result has an asymptote at the max value.
    /// </summary>
    /// <param name="x">Current value that the soft limit is imposed on</param>
    /// <param name="max">Absolute maximum value of the result</param>
    /// <param name="expFactor">Scaling factor used in the exponential function (higher means the limit is approached more quickly)</param>
    /// <returns>Soft limit between 0 and max</returns>
    private float ExponentialLimit(float x, float max, float expFactor)
    {
        return Mathf.Min(x, max * (1 - Mathf.Exp(-x * expFactor / max)));
    }
}