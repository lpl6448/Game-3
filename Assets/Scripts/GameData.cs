using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameData
{
    public static State gameState;
    public static State prevState;
    public static Characters targetChar;
    public static bool wonGolf;
    public static bool fromGolf;
    public static bool showCredits;

    public static SerializableDictionary<Characters, bool[]> progressFlags;

    static GameData()
    {
        gameState = State.Title;

        //Build progress flag dictionary if it wasn't loaded in
        if (progressFlags == null)
        {
            progressFlags = new SerializableDictionary<Characters, bool[]>(); // Golf at index 1

            progressFlags.Add(Characters.NONE, new bool[2]);
            progressFlags.Add(Characters.Molly, new bool[2]);
            progressFlags.Add(Characters.Marcone, new bool[2]);
            progressFlags.Add(Characters.LC, new bool[2]);
        }
    }
}
