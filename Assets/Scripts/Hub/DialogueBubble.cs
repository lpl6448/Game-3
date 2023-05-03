using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField]
    private Vector3 bubbleOffset;

    [SerializeField]
    private float scaleLerpFactor;

    [SerializeField]
    private float minimumScale;
    [SerializeField]
    private float maximumScale;

    private Character selectedCharacter;
    private bool active;
    private float currentScale = 0;

    public void Activate(Character selectedCharacter)
    {
        active = true;
        this.selectedCharacter = selectedCharacter;
    }

    private void LateUpdate()
    {
        // Move the bubble above the selected character
        if (selectedCharacter != null)
        {
            Vector3 characterDir = (selectedCharacter.transform.position - Camera.main.transform.position).normalized;
            Vector3 right = new Vector3(characterDir.z, 0, -characterDir.x).normalized;
            Vector3 up = Vector3.up;
            Vector3 forward = new Vector3(characterDir.x, 0, characterDir.z).normalized;
            transform.position = selectedCharacter.transform.position + right * bubbleOffset.x + up * bubbleOffset.y + forward * bubbleOffset.z;
        }

        // Animate the dialogue bubble scale
        if (active)
        {
            float goalScale = active ? maximumScale : minimumScale;
            currentScale = Mathf.Lerp(currentScale, goalScale, 1 - Mathf.Exp(-scaleLerpFactor * Time.deltaTime));
            if (currentScale > Mathf.Lerp(minimumScale, maximumScale, 0.05f))
                transform.localScale = Vector3.one * currentScale;
            else
                transform.localScale = Vector3.zero;
        }
        else
        {
            currentScale = 0;
            transform.localScale = Vector3.zero;
        }

        // Reset active for next frame
        active = false;

        // The dialogue bubble should always face the camera
        transform.forward = Camera.main.transform.forward;
    }
}
