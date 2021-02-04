using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IDropHandler,IDroppable,IPointerUpHandler
{
    private SpriteRenderer block;
    private GameManager gameManager;
    private SpriteRenderer highLight;
    private Color originalColor;

    private void Start() {
        var tempArr = GetComponentsInChildren<SpriteRenderer>();
        foreach(var temp in tempArr)
            if(temp.gameObject == gameObject)
                highLight = temp;
        foreach(var temp in tempArr)
            if(temp.transform.parent == transform)
                block = temp;
        highLight.enabled = false;
        if(block != null) originalColor = block.color;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        ChangeColor(true);
    }

    private void ChangeColor(bool isHighLight) {
        if(block == null) {
            gameManager.Selected = null;
        } else {
            if(isHighLight) {
                gameManager.Selected = GetComponent<IMySelectable>();
                block.color = HighlightColor(block.color);
            } else { block.color = originalColor; }
        }
    }

    private static Color HighlightColor(Color color) {
        var newColor = color;
        var colorDic =
            new Dictionary<string, float> {{"r", newColor.r}, {"g", newColor.g}, 
                {"b", newColor.b}, {"a", newColor.a}};
        var i = colorDic.Take(3).OrderBy(a => a.Value).Last().Key;
        switch(i) {
            case "r":
                newColor.r = 1;
                break;
            case "g":
                newColor.g = 1;
                break;
            case "b":
                newColor.b = 1;
                break;
        }
        return newColor;
    }

    public void RemoveSelect() {
        if(block !=null) ChangeColor(false);
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
                this.originalColor = wall.originalColor;
            } else { tempBlock.transform.parent = gameManager.originalParent.transform; }
            tempBlock.transform.localPosition = Vector3.zero;
        }
    }

    public bool IsDroppable() {
       return transform.childCount == 0;
    }
}
