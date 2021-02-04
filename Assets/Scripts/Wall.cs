using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler
    ,IPointerExitHandler,IDragHandler,IDropHandler,IDroppable,IPointerUpHandler
{
    private SpriteRenderer tile;
    private GameManager gameManager;
    private SpriteRenderer WallHighLightSprite;
    private Color originalColor;
    private bool isDragable = false;

    private void Start() {
        var tempArr = GetComponentsInChildren<SpriteRenderer>();
        foreach(var temp in tempArr)
            if(temp.gameObject == gameObject)
                WallHighLightSprite = temp;
        foreach(var temp in tempArr)
            if(temp.transform.parent == transform)
                tile = temp;
        WallHighLightSprite.enabled = false;
        if(tile != null) originalColor = tile.color;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        ChangeColor(true);
    }

    public void RemoveSelect() {
        if(tile !=null) ChangeColor(false);
        isDragable = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        var target = eventData.pointerCurrentRaycast.gameObject;
        if(tile != null) {
            var blockTransform = tile.transform;
            if(target.GetComponent<IDroppable>() == null) blockTransform.parent = transform;
            blockTransform.localPosition = Vector3.zero;
            blockTransform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        WallHighLightSprite.enabled = true;
        gameManager.targetCanSpawn = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        WallHighLightSprite.enabled = false;
        gameManager.targetCanSpawn = true;
    }

    public void OnDrag(PointerEventData eventData) {
        if(tile != null && isDragable) {
            gameManager.originalParent = gameObject;
            tile.transform.parent = null;
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = -5f;
            tile.transform.position = v3;
            tile.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }
    
    public void OnDrop(PointerEventData eventData) {
        var source = eventData.pointerDrag.gameObject;
        var wall = source.GetComponent<Wall>();
        if(wall != null&& wall.isDragable) {
            var tempBlock = wall.tile;
            if(wall.tile != null) {
                if(IsDroppable()) {
                    tempBlock.transform.parent = transform;
                    tile = tempBlock;
                    wall.tile = null;
                    originalColor = wall.originalColor;
                    tile.transform.localScale = new Vector3(1, 1, 1); //Set highlight scale to normal.
                    RemoveSelect(); //Cancel Selected.
                } else { tempBlock.transform.parent = gameManager.originalParent.transform; }
                tempBlock.transform.localPosition = Vector3.zero;
            }
        }
    }

    public bool IsDroppable() {
       return transform.childCount == 0;
    }


    private void ChangeColor(bool isHighLight) {
        if(tile == null) {
            gameManager.Selected = null;
        } else {
            if(isHighLight) {
                gameManager.Selected = GetComponent<IMySelectable>();
                tile.color = HighlightColor(tile.color);
                isDragable = true;
            } else { tile.color = originalColor; }
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
}
