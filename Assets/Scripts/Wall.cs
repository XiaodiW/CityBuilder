using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IDropHandler,IDroppable
{
    private SpriteRenderer block;
    private GameManager gameManager;
    private SpriteRenderer highLight;
    private bool isDroppable;

    private void Start() {
        var tempArr = GetComponentsInChildren<SpriteRenderer>();
        foreach(var temp in tempArr) {
            if(temp.gameObject == this.gameObject) highLight = temp;
        }
        foreach(var temp in tempArr) {
            if(temp.transform.parent == this.transform) block = temp;
        }
        highLight.enabled = false;
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }
    
    void ChangeColor() {
        gameManager.Selected = GetComponent<IMySelectable>();
        if(block !=null) block.color = Color.black;
    }
    
    public void RemoveSelect() {
        if(block !=null) block.color = Color.gray;
    }
    
    /*public void OnPointerDown(PointerEventData eventData) {
        if(block != null) {
            Debug.Log($"PonterDown:{eventData.pointerCurrentRaycast.gameObject.name}");
            gameManager.dragged = block.gameObject;
            block.transform.parent = null;
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = -5f;
            block.transform.position = v3;
        }
    }*/

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        highLight.enabled = true;
        isDroppable = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        highLight.enabled = false;
        isDroppable = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if(block != null) {
            gameManager.dragged = block.gameObject;
            block.transform.parent = null;
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = -5f;
            block.transform.position = v3;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        var target = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log($"Target: {target.name}");
        var blockTransform = block.transform;
        blockTransform.parent = target.GetComponent<IDroppable>() != null ? target.transform : this.transform;
        blockTransform.localPosition = Vector3.zero;
    }

    public bool IsDroppable() {
        return isDroppable;
    }
}
