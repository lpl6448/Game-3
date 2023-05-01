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

    [SerializeField]
    [Tooltip("List of levels in Molly's Challenge")]
    private GolfLevel[] mollyChallenge;
    [SerializeField]
    [Tooltip("List of levels in Marcone's Challenge")]
    private GolfLevel[] marconeChallenge;
    [SerializeField]
    [Tooltip("List of levels in Lacuna and Toot-Toot's Challenge")]
    private GolfLevel[] ltChallenge;

    private void Awake()
    {
        if (!hasInitialized)
        {
            if(GameData.targetChar!=Characters.NONE)
            {
                switch(GameData.targetChar)
                {
                    case Characters.Molly:
                        SetCharacterChallenge(mollyChallenge);
                        break;
                    case Characters.Marcone:
                        SetCharacterChallenge(marconeChallenge);
                        break;
                    case Characters.LC:
                        SetCharacterChallenge(ltChallenge); 
                        break;
                }
            }
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

    private void SetCharacterChallenge(GolfLevel[] levels)
    {
        string[] levelNames;
        levelNames = new string[levels.Length];
        for(int i=0; i<levels.Length; i++)
            levelNames[i] = levels[i].gameObject.name;
        GolfLevelManager.PrepareLevelSet(levelNames);
    }
}