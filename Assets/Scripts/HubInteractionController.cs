using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HubInteractionController : MonoBehaviour
{
    private EventSystem m_EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.gameState == State.Hub)
        {
            RaycastHit hit;
            //Send a ray from the camera to the mouseposition
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Create a raycast from the Camera to the mouse and output what it hits
            if (Physics.Raycast(ray, out hit))
            {
                //Check the hit GameObject has a Collider and that the GameObject is "Clickable"
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Clickable"))
                {
                    //Click a GameObject to return that GameObject your mouse pointer hit
                    GameObject m_MyGameObject = hit.collider.gameObject;
                    //Set this GameObject as the currently selected in the EventSystem
                    m_EventSystem.SetSelectedGameObject(m_MyGameObject);
                    m_MyGameObject.GetComponent<InteractableObject>().Hovered();
                    //Output the current selected GameObject's name to the console
                }
            }
            else
                m_EventSystem.SetSelectedGameObject(null);

            if (Input.GetMouseButtonDown(0) && m_EventSystem.currentSelectedGameObject != null)
            {
                GameData.targetChar = m_EventSystem.currentSelectedGameObject.GetComponent<InteractableObject>().Character;
                m_EventSystem.currentSelectedGameObject.GetComponent<InteractableObject>().Clicked();
            }
        }
    }
}
