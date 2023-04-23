using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that reacts when clicked on and highlights when hovered
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    string itemName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Each specific class of interactable item will leverage their own clicked method
    /// </summary>
    public virtual void Clicked()
    {
        Debug.Log($"Hi! I'm {itemName}");
    }

    /// <summary>
    /// Give an object a glowing highlight when the mouse hovers over it
    /// </summary>
    public virtual void Hovered()
    {
        //Highlight the item
    }
}
