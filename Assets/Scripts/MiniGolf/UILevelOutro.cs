using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UILevelOutro : MonoBehaviour
{
    private static readonly Dictionary<int, string> parDifferenceTable = new Dictionary<int, string>()
    {
        { -3, "Double Eagle" },
        { -2, "Eagle" },
        { -1, "Birdie" },
        { 0, "Par" },
        { 1, "Bogey" },
        { 2, "Double Bogey" },
        { 3, "Triple Bogey" },
    };
    private static readonly string holeInOneText = "Hole in One!";
    private static readonly string defaultUnderText = "Mega Eagle";
    private static readonly string defaultOverText = "Mega Bogey";

    [SerializeField]
    private Animation outroAnimation;

    [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField]
    private Image[] statusDividers;

    [SerializeField]
    private Color[] statusTextColors;

    [SerializeField]
    private Image continueBackground;

    [SerializeField]
    private TextMeshProUGUI continueText;

    [SerializeField]
    private Color continueLoseColor;

    [SerializeField]
    private Color continueWinColor;

    [SerializeField]
    private GameObject giveUpButton;

    public void AnimateOutro(int par, int strokes, bool hasNewLevel)
    {
        int parDiff = strokes - par;
        bool won = strokes <= par;
        string parDiffName;
        if (strokes == 1)
            parDiffName = holeInOneText;
        else
        {
            if (!parDifferenceTable.TryGetValue(parDiff, out parDiffName))
                if (strokes < par)
                    parDiffName = defaultUnderText;
                else
                    parDiffName = defaultOverText;
        }

        Color statusTextColor = statusTextColors[System.Math.Sign(parDiff) + 1];
        statusText.text = parDiffName;
        statusText.color = statusTextColor;
        foreach (Image divider in statusDividers)
        {
            Color color = statusTextColor;
            color.a = divider.color.a;
            divider.color = color;
        }

        if (won)
        {
            continueText.text = "Continue";
            continueBackground.color = continueWinColor;
            giveUpButton.SetActive(hasNewLevel);
        }
        else
        {
            continueText.text = "Try again";
            continueBackground.color = continueLoseColor;
            giveUpButton.SetActive(true);
        }

        outroAnimation.Play();
    }
}