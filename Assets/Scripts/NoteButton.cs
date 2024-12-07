using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteButton : Selectable, IPointerClickHandler
{
    public Sprite onImage;
    public Sprite offImage;
    private bool active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        active = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        active = !active;
        if (active)
        {
            GetComponent<Image>().sprite = onImage;
        }
        else 
        {
            GetComponent<Image>().sprite = offImage;
        }
     
    }

    void Update()
    {
        
    }
}
