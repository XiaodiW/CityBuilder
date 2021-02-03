using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IMySelectable ,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    private SpriteRenderer highLight;

    private void Start() {
        var tempArr = GetComponentsInChildren<SpriteRenderer>();
        highLight = tempArr[0].gameObject == this.gameObject ? tempArr[0] : tempArr[1];
        spriteRenderer = tempArr[0].gameObject == this.gameObject ? tempArr[1] : tempArr[0];
        highLight.enabled = false;
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
    }
    
    void ChangeColor() {
        gameManager.Selected = GetComponent<IMySelectable>();
        spriteRenderer.color = Color.black;
    }
    
    public void RemoveSelect() {
        spriteRenderer.color = Color.gray;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        gameManager.dragged = spriteRenderer.gameObject;
        spriteRenderer.transform.parent = null;
        var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.z = -5f;
        spriteRenderer.transform.position = v3;
    }

    public void OnPointerUp(PointerEventData eventData) {
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        highLight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        highLight.enabled = false;
    }
}
