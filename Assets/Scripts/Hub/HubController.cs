using Assets.Scripts.Enums;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubController : MonoBehaviour
{
    //Overlay objects
    [SerializeField] private GameObject dialogueOverlay;
    [SerializeField] private GameObject pauseOverlay;

    [SerializeField] private DialogueManager dialogueManager;

    //UI elements
    [SerializeField] private Image charSprite;

    [SerializeField] private WarpEffect warpEffect;

    //Chacter gameObjects in scene
    [SerializeField] private List<Character> loadSceneChars;
    private Dictionary<Characters, Character> sceneChars;

    // Start is called before the first frame update
    void Start()
    {
        warpEffect.StartCoroutine(warpEffect.WarpCameraIn());

        sceneChars = new Dictionary<Characters, Character>();
        sceneChars.Add(Characters.Molly, loadSceneChars[0]);
        sceneChars.Add(Characters.Marcone, loadSceneChars[1]);
        sceneChars.Add(Characters.LC, loadSceneChars[2]);
        //Check if intro cutscene has been played, play it if not
        if (!GameData.progressFlags[Characters.NONE][0])
        {
            GameData.gameState = State.Dialogue;
            dialogueOverlay.SetActive(true);
            dialogueManager.CallDialogueSequence(sceneChars, Characters.NONE, DLists.Intro);
        }
        //Handle if the scene is entered from golf
        if (GameData.fromGolf)
        {
            GameData.gameState = State.Dialogue;
            dialogueOverlay.SetActive(true);
            dialogueManager.CallDialogueSequence(sceneChars, GameData.targetChar);
        }

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
                    dialogueOverlay.SetActive(false);
                    break;
                case State.Dialogue:
                    dialogueOverlay.SetActive(true);
                    dialogueManager.CallDialogueSequence(sceneChars, GameData.targetChar);
                    break;
                case State.Paused:
                    pauseOverlay.SetActive(true);
                    break;
                case State.Golf:
                    //Exists for sake of the state machine
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
    private void ChangeDialogueChar(Emotions emotion)
    {
        charSprite.material = sceneChars[GameData.targetChar].GetSprite(emotion);
    }
}
