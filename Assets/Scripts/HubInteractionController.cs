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
        RaycastHit hit;
        //Send a ray from the camera to the mouseposition
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Create a raycast from the Camera and output anything it hits
        if (Physics.Raycast(ray, out hit))
            //Check the hit GameObject has a Collider
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Clickable"))
            {
                //Click a GameObject to return that GameObject your mouse pointer hit
                GameObject m_MyGameObject = hit.collider.gameObject;
                //Set this GameObject you clicked as the currently selected in the EventSystem
                m_EventSystem.SetSelectedGameObject(m_MyGameObject);
                //Output the current selected GameObject's name to the console
                Debug.Log(m_EventSystem.currentSelectedGameObject);
            }
    }
}
