using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubController : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueOverlay;
    [SerializeField]
    private GameObject pauseOverlay;

    // Start is called before the first frame update
    void Start()
    {
        GameData.prevState = GameData.gameState;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameData.gameState != GameData.prevState)
        {
            switch(GameData.gameState)
            {
                case State.Title:
                    //go to title screen;
                    break;
                case State.Hub:
                    //Add stuff if needed
                    break;
                case State.Dialogue:
                    dialogueOverlay.SetActive(true);
                    break;
                case State.Paused:
                    pauseOverlay.SetActive(true);
                    break;
                case State.Golf:
                    //Load golf scene
                    break;
            }
        }
        switch(GameData.gameState)
        {
            case State.Hub:
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseOverlay.SetActive(true);
                    GameData.gameState = State.Paused;
                }
                break;
            case State.Dialogue:
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    dialogueOverlay.SetActive(false);
                    GameData.gameState = State.Hub;
                }
                break;
            case State.Paused:
                if (Input.GetKeyDown(KeyCode.Escape)) 
                {
                    pauseOverlay.SetActive(false);
                    GameData.gameState = State.Hub;
                }
                break;
        }
        GameData.prevState = GameData.gameState;
    }

    /// <summary>
    /// Change the sprite of the character being spoken to with the targetChar field
    /// </summary>
    private void ChangeDialogueSprite()
    {
        switch(GameData.targetChar)
        {
            case Characters.Molly:
                //Switch to molly
                break;
            case Characters.Marcone:
                //Switch to marcone
                break;
            case Characters.LC:
                //Switch to Lacuna and Toot Toot
                break;
        }
    }
}
