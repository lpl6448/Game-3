using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameData
{
    public static State gameState;
    public static State prevState;
    public static Characters targetChar;

    public static Dictionary<Characters, bool[]> progressFlags;

    static GameData()
    {
        gameState = State.Hub;

        //Build progress flag dictionary
        progressFlags = new Dictionary<Characters, bool[]>();

        progressFlags.Add(Characters.Molly, new bool[3]);
        progressFlags.Add(Characters.Marcone, new bool[3]);
        progressFlags.Add(Characters.LC, new bool[2]);
    }
}
