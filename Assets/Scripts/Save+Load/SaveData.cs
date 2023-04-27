using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SerializableDictionary<Characters, bool[]> progressFlags;

    public SaveData()
    {
        progressFlags = new SerializableDictionary<Characters, bool[]>();

        progressFlags.Add(Characters.Molly, new bool[3]);
        progressFlags.Add(Characters.Marcone, new bool[3]);
        for (int i = 0; i < 3; i++)
        {
            progressFlags[Characters.Molly][i] = false;
            progressFlags[Characters.Marcone][i] = false;
        }
        progressFlags.Add(Characters.LC, new bool[3]);
        for (int i = 0; i < 3; i++)
            progressFlags[Characters.LC][i] = false;
    }
}
