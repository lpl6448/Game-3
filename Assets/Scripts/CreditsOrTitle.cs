using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsOrTitle : MonoBehaviour
{
    public void Credits()
    {
        GameData.showCredits = true;
        SceneManager.LoadScene("Title");
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
