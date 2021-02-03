using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IPointerClickHandler
{
    int index;

    private SpriteRenderer _spriteRenderer;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }

    void Start()
    {
        index = 0;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
 
    void ChangeColor()
    {
        if (index == 0)
        {
            _spriteRenderer.color = Color.black;
        }
        else
        {
            _spriteRenderer.color = Color.gray;
        }
           
        index = index == 0 ? 1 : 0;
    }
}
