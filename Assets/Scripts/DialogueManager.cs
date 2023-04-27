using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueBox;
    [SerializeField] private Button option1;
    [SerializeField] private Button option2;

    private Characters speaker;

    void Start()
    {
        //option1.onClick.AddListener(ToGolf);
        //option2.onClick.AddListener(ToGolf);
    }

    public void ToGolf()
    {
        //Update text to play golf
        dialogueBox.text = "Wanna play golf?";
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
        dialogueBox.text = "Gah! I lost";
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

    public void UpdateToTarget(Characters target)
    {
        speaker = target;
        if (GameData.progressFlags[target][0])
        {
            if (GameData.progressFlags[target][1])
            {
                if (GameData.progressFlags[target][2])
                {
                    
                    return;
                }
                ResetButtons();
                AssignSameClick(AlreadyWon);
                dialogueBox.text = "You already beat me";
                option1.GetComponentInChildren<TextMeshProUGUI>().text = "Ok";
                option2.GetComponentInChildren<TextMeshProUGUI>().text = "Bye";
                return;
            }
            ResetButtons();
            option1.onClick.AddListener(Confirm);
            option2.onClick.AddListener(Decline);
            dialogueBox.text = "Wanna play golf?";
            option1.GetComponentInChildren<TextMeshProUGUI>().text = "Yes";
            option2.GetComponentInChildren<TextMeshProUGUI>().text = "No";
            return;
        }
        ResetButtons();
        AssignSameClick(ToGolf);
        dialogueBox.text = "Hello Harry";
        option1.GetComponentInChildren<TextMeshProUGUI>().text = "Howdy!";
        option2.GetComponentInChildren<TextMeshProUGUI>().text = "Hey there!";
    }

    /// <summary>
    /// Helper method to remove button onClicks
    /// </summary>
    private void ResetButtons()
    {
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
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
