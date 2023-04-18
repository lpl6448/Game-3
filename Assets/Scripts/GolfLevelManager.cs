using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Stores the level to load and handles golf scene loading
/// </summary>
public static class GolfLevelManager
{
    // List containing GameObject names of the levels to be played
    // When the MiniGolf scene is loaded, the first level from the array is played.
    // It is only removed once the player has beaten the level.
    // (this is a static variable so that it can be set from other scenes' scripts)
    public static readonly Queue<string> LevelQueue = new Queue<string>();

    // String containing the GameObject name for the current level to load
    // If it is null, it just plays the next level in the queue
    public static string OverrideCurrentLevel;

    public static string GetLevel()
    {
        if (OverrideCurrentLevel == null)
            if (LevelQueue.Count > 0)
                return LevelQueue.Peek();
            else
                return null;
        else
            return OverrideCurrentLevel;
    }

    public static void LoadMiniGolfScene()
    {
        SceneManager.LoadScene("MiniGolf");
    }
    public static void CompleteLevel()
    {
        if (OverrideCurrentLevel != null)
        {
            if (LevelQueue.Count > 0 && LevelQueue.Peek() == OverrideCurrentLevel)
                LevelQueue.Dequeue();
            OverrideCurrentLevel = null;
        }
        else if (LevelQueue.Count > 0)
            LevelQueue.Dequeue();
    }
}