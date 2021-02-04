using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IDropHandler,IDroppable,IPointerUpHandler
{
    private SpriteRenderer block;
    private GameManager gameManager;
    private SpriteRenderer highLight;

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
        if(block !=null) ChangeColor();
    }
    
    void ChangeColor() {
        gameManager.Selected = GetComponent<IMySelectable>();
        block.color = Color.black;
    }
    
    public void RemoveSelect() {
        if(block !=null) block.color = Color.gray;
    }

    public void OnPointerUp(PointerEventData eventData) {
        var target = eventData.pointerCurrentRaycast.gameObject;
        if(block != null) {
            var blockTransform = block.transform;
            if(target.GetComponent<IDroppable>() == null) blockTransform.parent = transform;
            blockTransform.localPosition = Vector3.zero;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        highLight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        highLight.enabled = false;
    }

    public void OnDrag(PointerEventData eventData) {
        gameManager.originalParent = gameObject;
        block.transform.parent = null;
        var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.z = -5f;
        block.transform.position = v3;
    }

    public void OnDrop(PointerEventData eventData) {
        var source = eventData.pointerDrag.gameObject;
        var wall = source.GetComponent<Wall>();
        if(wall != null) {
            var tempBlock = wall.block;
            if(IsDroppable()) {
                tempBlock.transform.parent = transform;
                block = tempBlock;
                wall.block = null;
            } else { tempBlock.transform.parent = gameManager.originalParent.transform; }
            tempBlock.transform.localPosition = Vector3.zero;
        }
    }

    public bool IsDroppable() {
       return transform.childCount == 0;
    }
}
