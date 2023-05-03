using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGolfInstructions : MonoBehaviour
{
    [SerializeField] private UIGolfInstructionPanel[] panels;

    public void Initialize()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].Activate();
            if (GameData.miniGolfInstructionsIn[panels[i].StateIndex])
                panels[i].SlideIn();
        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].Deactivate();
    }
}