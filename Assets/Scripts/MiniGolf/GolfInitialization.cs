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
            if (!GolfLevelManager.HasNewLevel())
            {
                if (testLevel != null)
                    GolfLevelManager.PrepareLevelSet(testLevel.gameObject.name);
                else
                {
                    string[] levelNames = new string[levelsToPlay.Length];
                    for (int i = 0; i < levelsToPlay.Length; i++)
                        levelNames[i] = levelsToPlay[i].gameObject.name;
                    GolfLevelManager.PrepareLevelSet(levelNames);
                }
            }

            hasInitialized = true;
        }
    }
}