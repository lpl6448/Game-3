using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the game flow of the golf game mode (level management, stroke/par counting, etc.)
/// </summary>
public class GolfGameManager : MonoBehaviour
{
    // Reference to the golf ball managed by this script
    [SerializeField]
    private GolfBallController ball;

    // Reference to the camera controller in the scene
    [SerializeField]
    private GolfCameraController cameraController;

    // Current level (serialized into the inspector for now, but it will not be once we start this scene from the hub world)
    [SerializeField]
    private GolfLevel currentLevel;

    // Last position that the ball was "safe" or resting at
    private Vector3 lastBallPosition;

    // Whether the ball is currently out of bounds and in the process of respawning
    private bool isRespawning = false;

    /// <summary>
    /// For now, begin the level as soon as this scene is initialized
    /// </summary>
    private void Start()
    {
        BeginLevel(currentLevel);
    }

    /// <summary>
    /// Starts the specified level by spawning the ball there and moving the camera
    /// </summary>
    /// <param name="level"></param>
    public void BeginLevel(GolfLevel level)
    {
        currentLevel = level;

        // Spawn ball
        ball.transform.position = currentLevel.BallSpawn.position;
        SaveBallPosition();
    }

    /// <summary>
    /// Saves the current ball position as a "safe" or resting location that it can be respawned at later
    /// </summary>
    public void SaveBallPosition()
    {
        lastBallPosition = ball.transform.position;
    }

    /// <summary>
    /// Respawns the ball after a couple seconds (to allow the camera to follow it and to allow
    /// the player to realize their mistake)
    /// </summary>
    public void RespawnBall()
    {
        // If the current level has no bounds, do not respawn the ball unless it goes below y = -10
        if (!currentLevel.HasBounds && ball.transform.position.y > -10)
            return;

        if (!isRespawning)
            StartCoroutine(RespawnBallCrt());
    }

    /// <summary>
    /// Coroutine used to wait a couple seconds before respawning the ball, while controlling
    /// camera movements during this period as well
    /// </summary>
    /// <returns></returns>
    private IEnumerator RespawnBallCrt()
    {
        isRespawning = true;

        cameraController.Frozen = true;

        yield return new WaitForSeconds(2);

        ball.transform.position = lastBallPosition;
        ball.Rest();

        cameraController.Frozen = false;

        yield return new WaitForFixedUpdate();
        isRespawning = false;
    }
}