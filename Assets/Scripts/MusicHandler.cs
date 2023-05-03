using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] golfObjs = GameObject.FindGameObjectsWithTag("GolfMusic");
        //Replace golf music with hub music
        if (golfObjs.Length>1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        GameObject[] hubObjs = GameObject.FindGameObjectsWithTag("HubMusic");
        if (hubObjs.Length > 0)
            Destroy(this.gameObject);
    }
}
