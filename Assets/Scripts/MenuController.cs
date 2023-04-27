using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void NewGame()
    {
        //TODO: Delete save file
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
