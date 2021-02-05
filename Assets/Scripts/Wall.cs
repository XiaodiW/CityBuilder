using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler
    ,IPointerExitHandler,IDragHandler,IDropHandler,IDroppable,IPointerUpHandler {
    public Tile tile;
    private SpriteRenderer tileRederer;
    private GameManager gameManager;
    private SpriteRenderer WallHighLightSprite;
    private Color originalTileColor;
    private bool isDragable = false;

    private void Start() {
        var tempArr = GetComponentsInChildren<SpriteRenderer>();
        foreach(var temp in tempArr)
            if(temp.gameObject == gameObject)
                WallHighLightSprite = temp;
        foreach(var temp in tempArr)
            if(temp.transform.parent == transform)
                tileRederer = temp;
        WallHighLightSprite.enabled = false;
        if(tileRederer != null) originalTileColor = tileRederer.color;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        ChangeColor(true);
    }

    public void RemoveSelect() {
        if(tileRederer !=null) ChangeColor(false);
        isDragable = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        var target = eventData.pointerCurrentRaycast.gameObject;
        if(tileRederer != null) {
            var blockTransform = tileRederer.transform;
            if(target.GetComponent<IDroppable>() == null) blockTransform.parent = transform;
            blockTransform.localPosition = Vector3.zero;
            blockTransform.localScale = new Vector3(1, 1, 1);
            blockTransform.GetComponent<SpriteRenderer>().sortingOrder = 0; 
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
        if(tileRederer == null || !isDragable) return;
        var tileTrans = tileRederer.transform;
        //Save the original parent in GameManager
        gameManager.originalParent = gameObject; 
        //Put the dragged tile to root
        tileTrans.parent = null; 
        var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Set the dragged tile a bit close to camera. Since the Grid X Rotate 45o , y & z should be equal to each other.
        v3.z = v3.y - 0.2f; 
        tileTrans.position = v3;
        tileTrans.GetComponent<SpriteRenderer>().sortingOrder = 10; //
        //Enlarge tile during Dragging;
        tileTrans.localScale = new Vector3(1.5f, 1.5f, 1); 
    }
    
    public void OnDrop(PointerEventData eventData) {
        var source = eventData.pointerDrag.gameObject;
        var wall = source.GetComponent<Wall>();
        if(wall == null || !wall.isDragable) return;
        var transferTile = wall.tileRederer;
        if(transferTile == null) return;
        if(IsDroppable()) {
            transferTile.transform.parent = transform; //Put the transferred Tile as child of this.
            tileRederer = transferTile; //Set this.tile with transferred tile.
            if(source != gameObject)
                wall.tileRederer = null; //Remove previous tile from previous Wall. (** If transferred to itself**)
            originalTileColor = wall.originalTileColor; //Set tiles Original Color.
            tileRederer.transform.localScale = new Vector3(1, 1, 1); //Set highlight scale to normal.
            RemoveSelect(); //Cancel Selected.
        } else {
            if(gameManager.originalParent != null) transferTile.transform.parent = gameManager.originalParent.transform;
        } //Send the Tile back to original wall
        transferTile.transform.localPosition = Vector3.zero; // LocalPosition should be Vector3.Zero.
        transferTile.GetComponent<SpriteRenderer>().sortingOrder = 0; 
    }

    public bool IsDroppable() {
       return transform.childCount == 0;
    }

    private void ChangeColor(bool isHighLight) {
        if(tileRederer == null) { gameManager.Selected = null; } else {
            if(isHighLight) {
                gameManager.Selected = GetComponent<IMySelectable>();
                tileRederer.color = HighlightColor(tileRederer.color);
                isDragable = true;
            } else { tileRederer.color = originalTileColor; }
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
