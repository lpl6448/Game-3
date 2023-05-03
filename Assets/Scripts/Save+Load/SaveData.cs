using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SerializableDictionary<Characters, bool[]> progressFlags;
    private Characters[] keys;
    private bool[][] values;

    public SaveData()
    {
        progressFlags = new SerializableDictionary<Characters, bool[]>();

        progressFlags.Add(Characters.Molly, new bool[3]);
        progressFlags.Add(Characters.Marcone, new bool[2]);
        progressFlags.Add(Characters.LC, new bool[2]);
    }
}
