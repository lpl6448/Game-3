using UnityEngine;

/// <summary>
/// This is a temporary class that initializes the GolfLevelManager levels.
/// Eventually, the other scenes in the game will handle this.
/// </summary>
public class GolfInitialization : MonoBehaviour
{
    private static bool hasInitialized = false;

    [Header("Hover over property names to see details")]

    [SerializeField]
    [Tooltip("If this is not empty, the Levels to Play list is ignored and only this level is played\n(hit R to replay multiple times)")]
    private GolfLevel testLevel;

    [SerializeField]
    [Tooltip("List of levels to play, only used if Test Level is empty")]
    private GolfLevel[] levelsToPlay;

    private void Awake()
    {
        if (!hasInitialized)
        {
            if (testLevel != null)
                GolfLevelManager.LevelQueue.Enqueue(testLevel.gameObject.name);
            else
                foreach (GolfLevel level in levelsToPlay)
                    GolfLevelManager.LevelQueue.Enqueue(level.gameObject.name);

            hasInitialized = true;
        }
    }
}