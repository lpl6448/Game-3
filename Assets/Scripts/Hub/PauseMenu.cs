using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu: MonoBehaviour
{
    /// <summary>
    /// Resume hub gameplay
    /// </summary>
    public void Resume()
    {
        GameData.gameState = State.Hub;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Quit to the title screen
    /// </summary>
    public void ToTitle()
    {
        GameData.gameState = State.Title;
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// Close game
    /// </summary>
    public void ToDesktop()
    {
        Application.Quit();
    }
}
