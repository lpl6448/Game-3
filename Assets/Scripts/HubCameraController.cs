using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubCameraController : MonoBehaviour
{
    private float fWidth;

    void Start()
    {
        fWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D) || 
            (Input.mousePosition.x>fWidth-100))
            LookRight();
        else if(Input.GetKey(KeyCode.A) ||
            (Input.mousePosition.x < 100))
            LookLeft();
    }

    /// <summary>
    /// Shift camera right when called
    /// </summary>
    public void LookRight()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(Mathf.Clamp(pos.x + 0.01f, -6.0f, 6.0f), pos.y,pos.z);
    }

    /// <summary>
    /// Shift camera left when called
    /// </summary>
    public void LookLeft()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(Mathf.Clamp(pos.x - 0.01f, -6.0f, 6.0f), pos.y, pos.z);
    }
}
