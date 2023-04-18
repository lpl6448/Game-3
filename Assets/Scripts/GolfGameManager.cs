using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the game flow of the golf game mode (level management, stroke/par counting, etc.)
/// </summary>
public class GolfGameManager : MonoBehaviour
{
    // Reference to the golf ball managed by this script
    [SerializeField]
    private GolfBallPhysics ball;

    // Reference to the camera controller in the scene
    [SerializeField]
    private GolfCameraController cameraController;

    // Reference to the putting input manager in the scene
    [SerializeField]
    private GolfPuttingInput golfInput;

    // Reference to the overlay (controlling the stroke and par information)
    [SerializeField]
    private GolfOverlay golfOverlay;

    // Current level (serialized into the inspector for now, but it will not be once we start this scene from the hub world)
    [SerializeField]
    private GolfLevel currentLevel;

    // Last position that the ball was "safe" or resting at
    private Vector3 lastBallPosition;

    // Whether the ball is currently out of bounds and in the process of respawning
    private bool isRespawning = false;

    // Whether the ball is at rest and the game state is going to activate input soon (like after a camera animation)
    private bool isWaitingToActivateInput = false;

    // Whether any incoming requests to activate the golf ball input should be delayed
    private bool blockPuttingInput = false;

    // Flag that is set whenever the level is completed to prevent level completion from being triggered multiple times
    private bool completedLevel = false;

    // Counter for the number of strokes/putts the player has taken so far
    private int strokeCount = 0;

    /// <summary>
    /// For now, begin the level as soon as this scene is initialized
    /// </summary>
    private void Start()
    {
        // Try to load a level from the GolfLevelManager. If it cannot be loaded, load the current level as a fallback
        string levelName = GolfLevelManager.GetLevel();
        if (levelName != null)
        {
            GameObject levelObj = GameObject.Find(levelName);
            if (levelObj != null)
            {
                GolfLevel level = levelObj.GetComponent<GolfLevel>();
                if (level != null)
                    currentLevel = level;
            }
        }

        BeginLevel(currentLevel);
    }

    /// <summary>
    /// If waiting to activate input and input is no longer being blocked, activate input
    /// </summary>
    private void Update()
    {
        if (isWaitingToActivateInput && !blockPuttingInput)
        {
            isWaitingToActivateInput = false;
            golfInput.ActivateInput();
        }

        // When the player presses R, reload the current level
        if (Input.GetKeyDown(KeyCode.R))
        {
            GolfLevelManager.LoadMiniGolfScene();
        }

        // TEMPORARY: If the player presses a number, load a particular level
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GolfLevelManager.OverrideCurrentLevel = "ProbuilderTestLevel";
            GolfLevelManager.LoadMiniGolfScene();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GolfLevelManager.OverrideCurrentLevel = "WaterTestLevel";
            GolfLevelManager.LoadMiniGolfScene();
        }
    }

    /// <summary>
    /// Called by the onLaunch() event of the golf ball to increment the strokeCount by 1
    /// </summary>
    public void IncrementStroke()
    {
        strokeCount++;
        golfOverlay.UpdateStroke(strokeCount);
    }

    /// <summary>
    /// Starts the specified level by spawning the ball there and moving the camera
    /// </summary>
    /// <param name="level"></param>
    public void BeginLevel(GolfLevel level)
    {
        currentLevel = level;
        golfOverlay.UpdatePar(currentLevel.Par);
        golfOverlay.UpdateStroke(0);

        // Spawn ball
        ball.transform.position = currentLevel.BallSpawn.position;
        SaveBallPosition();

        StartCoroutine(LevelIntro());
    }

    /// <summary>
    /// Ends the level with a camera animation
    /// </summary>
    public void EndLevel()
    {
        if (!completedLevel)
        {
            completedLevel = true;
            StartCoroutine(LevelConclusion());
        }
    }

    /// <summary>
    /// Saves the current ball position as a "safe" or resting location that it can be respawned at later
    /// </summary>
    public void SaveBallPosition()
    {
        lastBallPosition = ball.transform.position;
    }

    /// <summary>
    /// Tells the game manager to activate user input on the golf ball once ready (like after a camera animation)
    /// </summary>
    public void RequestActivateInput()
    {
        if (blockPuttingInput)
            isWaitingToActivateInput = true;
        else
            golfInput.ActivateInput();
    }

    /// <summary>
    /// Respawns the ball after a couple seconds (to allow the camera to follow it and to allow
    /// the player to realize their mistake)
    /// </summary>
    public void RespawnBall(RespawnReason reason)
    {
        // If the current level has no bounds, do not respawn the ball unless it goes below y = -10
        if (reason == RespawnReason.OutOfBounds && !currentLevel.HasBounds && ball.transform.position.y > -10)
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

        cameraController.LockInput = true;

        yield return new WaitForSeconds(2);

        ball.transform.position = lastBallPosition;
        ball.Rest();

        cameraController.LockInput = false;

        yield return new WaitForFixedUpdate();
        isRespawning = false;
    }

    /// <summary>
    /// Runs an animation that introduces the level through camera animations
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelIntro()
    {
        blockPuttingInput = true;
        yield return null; // Wait one frame for now (so that the indicator has time to move to the starting ball position)

        // For now the camera animation parameters (position, rotation, etc.) are calculated here
        Vector3 focusDir = currentLevel.LevelIntroFocus != null
            ? (currentLevel.LevelIntroFocus.position - currentLevel.BallSpawn.position).normalized
            : Vector3.forward;
        focusDir += Vector3.down * 0.5f;
        focusDir.Normalize();

        Vector3 introCameraPos = currentLevel.IntroCameraPosition != null
            ? currentLevel.IntroCameraPosition.position
            : currentLevel.BallSpawn.position - focusDir * 10 + Vector3.up * 2;
        Quaternion introCameraRot = currentLevel.IntroCameraPosition != null
            ? currentLevel.IntroCameraPosition.rotation : Quaternion.LookRotation((focusDir * 10 - Vector3.up * 2).normalized);

        // Suspend camera input and animate it to the ball
        cameraController.Frozen = true;
        cameraController.transform.position = introCameraPos;
        cameraController.transform.rotation = introCameraRot;
        yield return new WaitForSeconds(2);
        yield return cameraController.AnimateToBall(introCameraPos, introCameraRot, cameraController.GoalTargetDistance, focusDir,
            2, t => Mathf.SmoothStep(0, 1, t));
        yield return new WaitForSeconds(0.5f);

        blockPuttingInput = false;
    }

    /// <summary>
    /// Runs an animation that zooms the camera out to tell the player that the level has been completed
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelConclusion()
    {
        // Permanently block input (until the scene is reset)
        blockPuttingInput = true;

        // For now camera animation parameters are calculated here
        float flatDis = 15;
        float upDis = 15;
        Vector3 conclCameraPos = cameraController.transform.position
            - cameraController.FlatForward * flatDis + Vector3.up * upDis;
        Quaternion conclCameraRot = Quaternion.LookRotation((cameraController.FlatForward * flatDis - Vector3.up * upDis).normalized);
        yield return cameraController.AnimateToStatic(conclCameraPos, conclCameraRot, 5, t => (1 - Mathf.Exp(-6 * t)) * (1 - Mathf.Exp(-6 * t)));

        // Load the next level (if there is one) by reloading the scene
        GolfLevelManager.CompleteLevel();
        if (GolfLevelManager.GetLevel() != null)
            GolfLevelManager.LoadMiniGolfScene();
    }
}