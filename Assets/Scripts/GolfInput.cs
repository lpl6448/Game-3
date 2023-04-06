using UnityEngine;

public class GolfInput : MonoBehaviour
{
    public GolfCameraController CameraController;
    public GolfBallIndicator Indicator;
    public GolfBallController BallController;

    private bool draggingBall = false;
    private bool draggingCamera = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Vector3.Dot(CameraController.transform.forward, mouseRay.direction) > 0.99f)
            {
                draggingBall = true;
                BallController.StartDrag();
            }
            else
            {
                draggingCamera = true;
                CameraController.StartDrag();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (draggingBall)
            {
                draggingBall = false;
                BallController.EndDrag();
            }
            if (draggingCamera)
            {
                draggingCamera = false;
                CameraController.EndDrag();
            }
        }
    }
}