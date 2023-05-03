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
    /// Reference to the DialogueBubble effect that can display above the character (only one per scene)
    /// </summary>
    [SerializeField]
    private DialogueBubble dialogueBubble;
    /// <summary>
    /// Dictionary that contains all sprites and their emotions key
    /// </summary>
    protected Dictionary<Emotions, Material> charMats;


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

    /// <summary>
    /// Pulls up dialogue box for specific character
    /// </summary>
    public override void Clicked()
    {
        base.Clicked();
        GameData.gameState = State.Dialogue;
        GameData.targetChar = character;
    }

    /// <summary>
    /// Makes a dialogue bubble appear above the character
    /// </summary>
    public override void Hovered()
    {
        base.Hovered();
        dialogueBubble.Activate(this);
    }

    public virtual Material GetSprite(Emotions emotion)
    {
        return charMats[emotion];
    }
}
