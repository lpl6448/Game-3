using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler Instance { get; private set; }

    private AudioSource src;

    [SerializeField] AudioClip wallBonk;
    [SerializeField] AudioClip putt;
    [SerializeField] AudioClip splash;


    private void Start()
    {
        Instance = this;
        src = gameObject.GetComponent<AudioSource>();
    }
    public void playWallBonk()
    {
        src.PlayOneShot(wallBonk);
    }
    public void playPutt()
    {
        src.PlayOneShot(putt);
    }
    public void PlaySplash()
    {
        src.PlayOneShot(splash);
    }
}
