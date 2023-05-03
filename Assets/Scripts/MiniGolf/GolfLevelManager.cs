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

    // Whether the intro camera/UI animation is enabled when the next level loads
    public static bool PlayIntroSequence { get; set; } = true;

    // Whether the warp effect should be played when the next level loads
    public static bool PlayWarpEffect { get; set; } = true;

    // Stores the last played level (as a fallback for if there is no next level to play)
    private static string lastCompletedLevel = null;

    public static bool hasInitialized = false;

    public static void PrepareLevelSet(params string[] levels)
    {
        LevelQueue.Clear();
        OverrideCurrentLevel = null;

        foreach (string level in levels)
            LevelQueue.Enqueue(level);
    }

    public static bool HasNewLevel()
    {
        if (OverrideCurrentLevel != null)
            return true;
        else
            return LevelQueue.Count > 0;
    }

    public static string GetLevel()
    {
        if (OverrideCurrentLevel == null)
            if (LevelQueue.Count > 0)
                return LevelQueue.Peek();
            else
                return lastCompletedLevel;
        else
            return OverrideCurrentLevel;
    }

    public static void LoadMiniGolfScene(bool playIntro)
    {
        PlayIntroSequence = playIntro;
        SceneManager.LoadScene("MiniGolf");
    }
    public static void CompleteLevel()
    {
        lastCompletedLevel = GetLevel();
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