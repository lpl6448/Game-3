using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //UI Componants
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Button option1;
    [SerializeField] private Button option2;
    [SerializeField] private Image continueArrow;
    [SerializeField] private Image dCharSprite;

    private TextMeshProUGUI dialogueText;
    private Button dialogueButton;

    private Characters speaker;

    //Lists to hold dialogue frames associated with characters or progressions
    private List<DialogueFrame> introDL = new List<DialogueFrame>();
    private List<DialogueFrame> mollyDL = new List<DialogueFrame>();
    private List<DialogueFrame> marconeDL = new List<DialogueFrame>();
    private List<DialogueFrame> lcDL = new List<DialogueFrame>();
    //List of loaded dialogue frame lists
    private List<DialogueFrame> targetFrameList;

    DialogueFrame currentFrame;

    [SerializeField] private List<Character> characterList;
    private Dictionary<Characters, Character> characterDict = new Dictionary<Characters, Character>();

    void Awake()
    {
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueButton = dialogueBox.GetComponent<Button>();
        characterDict.Add(Characters.Molly, characterList[0]);
        characterDict.Add(Characters.Marcone, characterList[1]);
        characterDict.Add(Characters.LC, characterList[2]);
    }

    public void ToGolf()
    {
        //Update text to play golf
        dialogueText.text = "Wanna play golf?";
        option1.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
        option2.GetComponentInChildren<TextMeshProUGUI>().text = "No";
        //Assign confirm to start golf and deny to not play golf to the button on clicks
        ResetButtons();
        option1.onClick.AddListener(Confirm);
        option2.onClick.AddListener(Decline);

        //Update progress flag 0 to true for speaker
        GameData.progressFlags[speaker][0] = true;
    }

    public void Confirm()
    {
        //TODO: Load correct golf levels
        Debug.Log("Woo played golf");
        ResetButtons();
        AssignSameClick(Winner);
        dialogueText.text = "Gah! I lost";
        option1.GetComponentInChildren<TextMeshProUGUI>().text = "Yippee";
        option2.GetComponentInChildren<TextMeshProUGUI>().text = "It eez what it eez";
    }

    /// <summary>
    /// Declines to play golf and closes dialogue overlay
    /// Also serves as a sort of helper function to close dialogue overlay
    /// </summary>
    public void Decline()
    {
        //Return to hub state
        GameData.gameState = State.Hub;
        gameObject.SetActive(false);
    }

    public void Winner()
    {
        //TODO: Dialogue for winning
        Debug.Log("Hey you won");
        //TEMP: Update progress flag 1 to true for speaker
        GameData.progressFlags[speaker][1] = true;
        Decline();

    }

    public void AlreadyWon()
    {
        //TODO: Dialogue if you already won
        Decline();
    }

    public void Continue(int nextFrame)
    {
        if(currentFrame.LineType==LineType.ToGolf)
        {
            //TODO: Implement going to golf
        }
        //If the next frame will be nothing, close the dialogue overlay and return to hub
        if(nextFrame==-1)
        {
            GameData.gameState = State.Hub;
            gameObject.SetActive(false);
            return;
        }
        //Change current frame to next frame and call to render it
        currentFrame = targetFrameList[nextFrame];
        RenderDialogueFrame();
    }

    public void CallDialogueSequence(Dictionary<Characters, Character> sceneChars, Characters target = Characters.NONE, DLists toRequest=DLists.Intro)
    {
        speaker = target;
        if(targetFrameList!=null)
            targetFrameList.Clear();
        //Request JSON objects based on speaker if there is one
        if (speaker != Characters.NONE)
        {
            switch(speaker)
            {
                case Characters.Molly:
                    targetFrameList = LoadDialogueList(DLists.Molly);
                    break;
                case Characters.Marcone:
                    targetFrameList = LoadDialogueList(DLists.Marcone);
                    break;
                case Characters.LC:
                    targetFrameList = LoadDialogueList(DLists.LT);
                    break;
            }
        }
        else
        {
            targetFrameList = LoadDialogueList(toRequest);
        }

        //Start dialogue interaction
        DetermineDialogueSequence();
        //RenderDialogueFrame will keep the dialogue running until the sequence ends
        RenderDialogueFrame();

        //if (GameData.progressFlags[target][0])
        //{
        //    if (GameData.progressFlags[target][1])
        //    {
        //        if (GameData.progressFlags[target][2])
        //        {

        //            return;
        //        }
        //        ResetButtons();
        //        AssignSameClick(AlreadyWon);
        //        dialogueText.text = "You already beat me";
        //        option1.GetComponentInChildren<TextMeshProUGUI>().text = "Ok";
        //        option2.GetComponentInChildren<TextMeshProUGUI>().text = "Bye";
        //        return;
        //    }
        //    ResetButtons();
        //    option1.onClick.AddListener(Confirm);
        //    option2.onClick.AddListener(Decline);
        //    dialogueText.text = "Wanna play golf?";
        //    option1.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
        //    option2.GetComponentInChildren<TextMeshProUGUI>().text = "No";
        //    return;
        //}
        //ResetButtons();
        //AssignSameClick(ToGolf);
        //dialogueText.text = "Hello Harry";
        //option1.GetComponentInChildren<TextMeshProUGUI>().text = "Howdy!";
        //option2.GetComponentInChildren<TextMeshProUGUI>().text = "Hey there!";
    }

    /// <summary>
    /// Update dialgoue screen elements with relevant material from currentFrame object
    /// </summary>
    private void RenderDialogueFrame()
    {
        ResetButtons();
        dialogueText.text = $"{currentFrame.Speaker}: ${currentFrame.Line}";
        //Only run response code if the prompt has responses available
        if (currentFrame.NextFrame2 != -1 || currentFrame.LineType == LineType.Respondable)
        {
            //Activate response objects
            option1.gameObject.SetActive(true);
            option2.gameObject.SetActive(true);
            //Fill text with the two responses
            option1.GetComponentInChildren<TextMeshProUGUI>().text = currentFrame.Response1;
            option1.GetComponentInChildren<TextMeshProUGUI>().text = currentFrame.Response2;
            //Assign buttons with onclicks that will bring up the next frame
            option1.onClick.AddListener(delegate { Continue(currentFrame.NextFrame1); });
            option2.onClick.AddListener(delegate { Continue(currentFrame.NextFrame2); });
        }
        //Otherwise, continuing will be handled by clicking the dialogue box
        else
        {
            continueArrow.gameObject.SetActive(true);
            dialogueButton.onClick.AddListener(delegate { Continue(currentFrame.NextFrame1); });
        }

        //Update the character portrait on screen (unless speaker is dresden)
        if(speaker!=Characters.Dresden)
        {
            dCharSprite.material = characterDict[speaker].GetSprite(currentFrame.Emotion);
        }
    }

    /// <summary>
    /// Takes in the dialogue list that should be loaded and returns that list
    /// </summary>
    /// <param name="listRequest"></param>
    /// <returns></returns>
    private List<DialogueFrame> LoadDialogueList(DLists listRequest)
    {
        //Create a FrameLoader object to load in dialogue
        FrameLoader loader = new FrameLoader();
        //Load in each dialogue frame list that was requested
        return loader.LoadDialogue(listRequest);
    }

    /// <summary>
    /// Search through targetFrameList for the frame that matches the requested special LineType
    /// </summary>
    /// <param name="searchFor"></param>
    private void GetCurrentFrame(LineType searchFor)
    {
        foreach(DialogueFrame frame in targetFrameList)
        {
            if(searchFor==frame.LineType)
            {
                currentFrame = frame;
                break;
            }
        }
    }

    /// <summary>
    /// Set current frame to the provided ID from the targetFrameList
    /// </summary>
    /// <param name="ID"></param>
    private void GetCurrentFrame(int ID)
    {
        currentFrame = targetFrameList[ID];
    }

    private void DetermineDialogueSequence()
    {
        if (speaker == Characters.NONE)
            return;
        switch(speaker)
        {
            case Characters.Molly:
                if (GameData.progressFlags[Characters.Molly][0])
                    GetCurrentFrame(LineType.AskToGolf);
                else if (GameData.progressFlags[Characters.Molly][1])
                    GetCurrentFrame(LineType.HasLost);
                else if (GameData.progressFlags[Characters.Molly][2])
                    //Hardcoded ID for the frame that Molly will prompt completing the game, could be implemented better
                    GetCurrentFrame(9);
                else
                    GetCurrentFrame(0);
                break;
            case Characters.Marcone:
            case Characters.LC:
                if (GameData.progressFlags[speaker][0])
                    GetCurrentFrame(LineType.AskToGolf);
                else if(GameData.progressFlags[speaker][1])
                    GetCurrentFrame(LineType.HasLost);
                else
                    GetCurrentFrame(0);
                break;
        }
    }


    /// <summary>
    /// Helper method to remove button onClicks
    /// </summary>
    private void ResetButtons()
    {
        //Reset the onclicks of each button
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        dialogueButton.onClick.RemoveAllListeners();
        //Hide all buttons
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        continueArrow.gameObject.SetActive(false);
    }

    /// <summary>
    /// Give both buttons the same onclick
    /// Primarily for pre-dialogue testing
    /// </summary>
    /// <param name="func"></param>
    private void AssignSameClick(UnityEngine.Events.UnityAction func)
    {
        option1.onClick.AddListener(func);
        option2.onClick.AddListener(func);
    }
}
