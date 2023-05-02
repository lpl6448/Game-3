using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
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
        //TODO: Bring up credits screen
        Debug.Log("Made by Caniac Games");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
