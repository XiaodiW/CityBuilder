using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour,IPointerClickHandler {
    private IMySelectable selected;

    public IMySelectable Selected {
        set {
            selected?.OnUnSelected();
            selected = value;
        } 
    }

    public void OnPointerClick(PointerEventData eventData) {
        Selected = null;
    }
}
