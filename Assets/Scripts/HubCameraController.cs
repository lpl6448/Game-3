using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubCameraController : MonoBehaviour
{
    private float fWidth;
    [SerializeField]
    private float rotSpeed;

    void Start()
    {
        fWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.x>fWidth-100)
            LookRight();
        else if(Input.mousePosition.x < 100)
            LookLeft();
    }

    /// <summary>
    /// Shift camera right when called
    /// </summary>
    public void LookRight()
    {
        if(transform.rotation.y < 0.3420216f)
            transform.Rotate(0.0f, rotSpeed, 0.0f);
    }

    /// <summary>
    /// Shift camera left when called
    /// </summary>
    public void LookLeft()
    {
        if (transform.rotation.y > -0.3420201f)
            transform.Rotate(0.0f, -rotSpeed, 0.0f);
    }
}
