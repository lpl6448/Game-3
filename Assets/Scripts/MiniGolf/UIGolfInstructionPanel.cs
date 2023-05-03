using UnityEngine;

public class UIGolfInstructionPanel : MonoBehaviour
{
    [SerializeField] private RectTransform chevronButton;

    [SerializeField] private RectTransform slidingTransform;

    [SerializeField] private float slidingInX;

    [SerializeField] private float slidingOutX;

    [SerializeField] private float deactivatedX;

    [SerializeField] public int StateIndex;

    private bool isIn = false;
    private bool deactivated = true;

    public void Activate()
    {
        deactivated = false;
    }
    public void Deactivate()
    {
        deactivated = true;
        SlideOut();
    }

    public void ToggleIn()
    {
        deactivated = false;
        isIn = !isIn;
        SaveState();
    }
    public void SlideIn()
    {
        deactivated = false;
        isIn = true;
        SaveState();
    }
    public void SlideOut()
    {
        isIn = false;
        SaveState();
    }

    private void Update()
    {
        float goalSlide = deactivated ? deactivatedX : isIn ? slidingInX : slidingOutX;
        float goalChevronRotation = isIn ? 0 : 180;

        slidingTransform.anchoredPosition = new Vector2(
            Mathf.Lerp(slidingTransform.anchoredPosition.x, goalSlide, 1 - Mathf.Exp(-6 * Time.unscaledDeltaTime)), slidingTransform.anchoredPosition.y);
        chevronButton.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(chevronButton.localEulerAngles.z, goalChevronRotation, 1 - Mathf.Exp(-8 * Time.unscaledDeltaTime)));
    }

    private void SaveState()
    {
        // Save state to GameData
        GameData.miniGolfInstructionsIn[StateIndex] = isIn;
    }
}