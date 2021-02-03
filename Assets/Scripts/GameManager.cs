using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour,IPointerClickHandler {
    private IMySelectable selected;
    public GameObject dragged;

    public IMySelectable Selected {
        set {
            selected?.RemoveSelect();
            selected = value;
        } 
    }

    public void OnPointerClick(PointerEventData eventData) {
        Selected = null;
    }
    
}
