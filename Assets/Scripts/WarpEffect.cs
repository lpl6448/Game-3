using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.SceneManagement;

public class WarpEffect : MonoBehaviour
{
    [SerializeField]
    private Animation warpAnimations;

    [SerializeField]
    private AnimationClip warpOutAnimation;

    [SerializeField]
    private AnimationClip warpInAnimation;

    public IEnumerator WarpCameraOut(string nextScene)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(nextScene);
        load.allowSceneActivation = false;
        warpAnimations.clip = warpOutAnimation;
        warpAnimations.Play();
        while (warpAnimations.isPlaying)
            yield return null;
        load.allowSceneActivation = true;
    }

    public IEnumerator WarpCameraIn()
    {
        warpAnimations.clip = warpInAnimation;
        warpAnimations.Play();
        while (warpAnimations.isPlaying)
            yield return null;
    }
}