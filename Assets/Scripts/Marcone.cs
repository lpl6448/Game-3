using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marcone : Character
{
    [SerializeField] private List<Material> nose1Mats = new List<Material>();
    [SerializeField] private List<Material> nose2Mats = new List<Material>();

    private int noseSize = 0;
    
    public void GrowNose()
    {
        //Grow nose
        if(charMats!=null)
            noseSize++;
        //Select which material list is for the new nose size
        List<Material> newNoseList= new List<Material>();
        if(noseSize==1)
        {
            newNoseList = nose1Mats;
        }
        else if(noseSize==2)
        {
            newNoseList= nose2Mats;
        }
        List<Emotions> keylist = new List<Emotions>();
        foreach(KeyValuePair<Emotions, Material> entry in charMats)
        {
            keylist.Add(entry.Key);
        }
        //Replace character materials with new nose size
        int iterator = 0;
        foreach(Emotions key in keylist)
        {
            charMats[key] = newNoseList[iterator];
            iterator++;
        }
    }
}
