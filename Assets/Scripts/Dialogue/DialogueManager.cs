using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    //Dialogue JSON Files
    [SerializeField] TextAsset introDialogueJSON;
    [SerializeField] TextAsset MollyDialogueJSON;
    [SerializeField] TextAsset MarconeDialogueJSON;
    [SerializeField] TextAsset LTDialogueJSON;

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

    public void Continue(int nextFrame)
    {
        if (currentFrame.LineType==LineType.ToGolf)
        {
            dialogueBox.SetActive(false);
            GameData.gameState = State.Golf;
            GolfLevelManager.hasInitialized = false;
            SceneManager.LoadScene("MiniGolf");
        }
        if (currentFrame.LineType == LineType.WonGolf || currentFrame.LineType == LineType.LostGolf)
            GameData.fromGolf = false;
        //If the next frame will be nothing, close the dialogue overlay and return to hub
        if(nextFrame==-1)
        {
            GameData.gameState = State.Hub;
            gameObject.SetActive(false);
            return;
        }
        //Change current frame to next frame and call to render it
        currentFrame = targetFrameList[nextFrame];
        //If currentFrame is an asktogolf frame, mark first word progress flag as true
        if (currentFrame.LineType == LineType.AskToGolf)
            GameData.progressFlags[GameData.targetChar][0] = true;
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
                    targetFrameList = LoadDialogueList(DLists.Molly, MollyDialogueJSON);
                    break;
                case Characters.Marcone:
                    targetFrameList = LoadDialogueList(DLists.Marcone, MarconeDialogueJSON);
                    break;
                case Characters.LC:
                    targetFrameList = LoadDialogueList(DLists.LT, LTDialogueJSON);
                    break;
            }
        }
        else
        {
            targetFrameList = LoadDialogueList(toRequest, introDialogueJSON);
        }

        //Start dialogue interaction
        DetermineDialogueSequence();
        //RenderDialogueFrame will keep the dialogue running until the sequence ends
        RenderDialogueFrame();
    }

    /// <summary>
    /// Update dialgoue screen elements with relevant material from currentFrame object
    /// </summary>
    private void RenderDialogueFrame()
    {
        ResetButtons();
        dialogueText.text = $"{currentFrame.Speaker}: {currentFrame.Line}";
        //Only run response code if the prompt has responses available
        if (currentFrame.NextFrame2 != -1 || currentFrame.LineType == LineType.Respondible)
        {
            //Activate response objects
            option1.gameObject.SetActive(true);
            option2.gameObject.SetActive(true);
            //Fill text with the two responses
            option1.GetComponentInChildren<TextMeshProUGUI>().text = currentFrame.Response1;
            option2.GetComponentInChildren<TextMeshProUGUI>().text = currentFrame.Response2;
            //Assign buttons with onclicks that will bring up the next frame
            option1.onClick.AddListener(delegate { Continue(currentFrame.NextFrame1); });
            if (currentFrame.NextFrame2 != -1)
                option2.onClick.AddListener(delegate { Continue(currentFrame.NextFrame2); });
            else
                option2.onClick.AddListener(delegate { Continue(currentFrame.NextFrame1); });
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
    private List<DialogueFrame> LoadDialogueList(DLists listRequest, TextAsset targetJSON)
    {
        //Create a FrameLoader object to load in dialogue
        FrameLoader loader = new FrameLoader();
        //Load in each dialogue frame list that was requested
        return loader.LoadDialogue(listRequest, targetJSON);
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
                return;
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

        //Handle starting dialogue after golf
        if(GameData.fromGolf)
        {
            //Call dialogue for winning and make this characters golf won flag as true
            if (GameData.wonGolf)
            {
                GetCurrentFrame(LineType.WonGolf);
                GameData.progressFlags[speaker][1] = true;
            }
            //Call lost golf dialogue
            else
                GetCurrentFrame(LineType.LostGolf);
            return;
        }

        switch(speaker)
        {
            case Characters.Molly:
                if (GameData.progressFlags[Characters.Molly][2])
                    //Hardcoded ID for the frame that Molly will prompt completing the game, could be implemented better
                    GetCurrentFrame(9);
                else if (GameData.progressFlags[Characters.Molly][1])
                    GetCurrentFrame(LineType.HasLost);
                else if (GameData.progressFlags[Characters.Molly][0])
                    GetCurrentFrame(LineType.AskToGolf);
                else
                    GetCurrentFrame(0);
                break;
            case Characters.Marcone:
            case Characters.LC:
                if (GameData.progressFlags[speaker][1])
                    GetCurrentFrame(LineType.HasLost);
                else if(GameData.progressFlags[speaker][0])
                    GetCurrentFrame(LineType.AskToGolf);
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
}
