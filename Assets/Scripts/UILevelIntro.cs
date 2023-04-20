using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UILevelIntro : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI parText;

    [SerializeField]
    private Animation introAnimation;

    public void AnimateIntro(GolfLevel level)
    {
        nameText.text = level.DisplayName;
        parText.text = "Par " + level.Par;

        introAnimation.Play();
    }
}