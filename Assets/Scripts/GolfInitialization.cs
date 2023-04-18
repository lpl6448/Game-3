using UnityEngine;

/// <summary>
/// This is a temporary class that initializes the GolfLevelManager levels.
/// Eventually, the other scenes in the game will handle this.
/// </summary>
public class GolfInitialization : MonoBehaviour
{
    private static bool hasInitialized = false;

    [SerializeField]
    private GolfLevel[] levelsToPlay;

    private void Awake()
    {
        if (!hasInitialized)
        {
            foreach (GolfLevel level in levelsToPlay)
                GolfLevelManager.LevelQueue.Enqueue(level.gameObject.name);
            hasInitialized = true;
        }
    }
}