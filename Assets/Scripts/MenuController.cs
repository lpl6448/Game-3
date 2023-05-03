using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Canvas creditOverlay;

    private void Start()
    {
        if (GameData.showCredits)
        {
            GameData.showCredits = false;
            Credits();
        }
    }

    public void NewGame()
    {
        //TODO: Delete save file
        foreach(KeyValuePair<Characters, bool[]> entry in GameData.progressFlags)
        {
            for(int i=0, length=entry.Value.Length; i<length; i++)
            {
                entry.Value[i] = false;
            }
        }

        GameData.gameState = State.Hub;
        SceneManager.LoadScene("CountryClub");
    }

    public void Continue()
    {
        GameData.gameState = State.Hub;
        SceneManager.LoadScene("CountryClub");
    }

    public void Credits()
    {
        creditOverlay.gameObject.SetActive(!creditOverlay.isActiveAndEnabled);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
