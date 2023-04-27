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

    //Chacter gameObjects in scene
    [SerializeField] private List<Character> loadSceneChars;
    private Dictionary<Characters, Character> sceneChars;

    // Start is called before the first frame update
    void Start()
    {
        sceneChars = new Dictionary<Characters, Character>();
        sceneChars.Add(Characters.Molly, loadSceneChars[0]);
        sceneChars.Add(Characters.Marcone, loadSceneChars[1]);
        sceneChars.Add(Characters.LC, loadSceneChars[2]);
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
                    dialogueManager.UpdateToTarget(GameData.targetChar);
                    ChangeDialogueChar(Emotions.Neutral);
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
    private void ChangeDialogueChar(Emotions emotion)
    {
        charSprite.material = sceneChars[GameData.targetChar].GetSprite(emotion);
    }
}
