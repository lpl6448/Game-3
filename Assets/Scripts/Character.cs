using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : InteractableObject
{
    /// <summary>
    /// Holds the speech bubble object associated with this character
    /// </summary>
    [SerializeField]
    protected GameObject speechBubble;

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
    protected Dictionary<Emotions, Material> charMats;

    protected bool hovering = false;


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

    protected void Update()
    {
        //if (!hovering)
        //    return;
        //hovering = false;
        //speechBubble.SetActive(false);
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

    public override void Hovered()
    {
        hovering = true;
        speechBubble.SetActive(true);
    }

    public override void UnHover()
    {
        hovering=false;
        speechBubble.SetActive(false);
    }

    public virtual Material GetSprite(Emotions emotion)
    {
        return charMats[emotion];
    }
}
