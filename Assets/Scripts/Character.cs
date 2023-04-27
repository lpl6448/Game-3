using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    /// <summary>
    /// List of sprites for different character emotions
    /// </summary>
    [SerializeField]
    private List<Material> loadCharMats;
    /// <summary>
    /// List of emotions that is parallel to the loadSprites field
    /// </summary>
    [SerializeField]
    private List<Emotions> spriteEmotions;
    /// <summary>
    /// Dictionary that contains all sprites and their emotions key
    /// </summary>
    private Dictionary<Emotions, Material> charMats;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Initialize and fill dictionary
        charMats = new Dictionary<Emotions, Material>();
        for(int i=0, spriteCount=loadCharMats.Count; i<spriteCount; i++) 
        {
            charMats.Add(spriteEmotions[i], loadCharMats[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Pulls up dialogue box for specific character
    /// </summary>
    public override void Clicked()
    {
        base.Clicked();
        GameData.gameState = State.Dialogue;
        GameData.targetChar = character;
    }

    public virtual Material GetSprite(Emotions emotion)
    {
        return charMats[emotion];
    }
}
