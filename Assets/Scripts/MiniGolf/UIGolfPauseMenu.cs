using UnityEngine;
using System.Collections;
using TMPro;

public class UIGolfPauseMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelTitleText;

    [SerializeField]
    private Animator animator;

    public bool Active { get; private set; }

    public bool CanInteract { get; private set; } = true;

    public void PauseMenuIn(GolfLevel level)
    {
        levelTitleText.text = level.DisplayName;
        animator.SetBool("In", true);
        Active = true;

        CanInteract = false;
        StartCoroutine(DelayCanInteract(65 / 60f));
    }
    public void PauseMenuOut()
    {
        animator.SetBool("In", false);
        Active = false;

        CanInteract = false;
        StartCoroutine(DelayCanInteract(45 / 60f));
    }

    private IEnumerator DelayCanInteract(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        CanInteract = true;
    }
}