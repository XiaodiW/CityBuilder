using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IDragHandler
{
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
 
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }
 
    void ChangeColor() {
        gameManager.Selected = GetComponent<IMySelectable>();
        spriteRenderer.color = Color.black;
    }
    
    public void OnUnSelected() {
        spriteRenderer.color = Color.gray;
    }

    public void OnDrag(PointerEventData eventData) {
        spriteRenderer.transform.position = Input.mousePosition;
    }
}
