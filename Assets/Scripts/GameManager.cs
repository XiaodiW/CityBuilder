using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour,IPointerClickHandler {
    private IMySelectable selected;
    [HideInInspector]public GameObject originalParent;
    private bool isSpawn;
    public Button spawanTrigger;
    public GameObject prefab;
    private GameObject isoMetricGrid;
    private Text modeText;
    public bool canSpawn;

    private void Start() {
        isoMetricGrid = GameObject.Find("IsoMetricGrid");
        modeText = spawanTrigger.GetComponentInChildren<Text>();
        modeText.text = isSpawn ? "Spawn Mode" : "Normal Mode";
    }

    public IMySelectable Selected {
        set {
            selected?.RemoveSelect();
            selected = value;
        } 
    }

    public void SpawnTrigger() {
        isSpawn = !isSpawn;
        modeText.text = isSpawn ? "Spawn Mode" : "Normal Mode";
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(isSpawn) {
            if(canSpawn) {
                var mousePosition = eventData.pointerCurrentRaycast.worldPosition;
                var instance = Instantiate(prefab, isoMetricGrid.transform);
                instance.transform.position = mousePosition;
                var tempPos = instance.transform.localPosition;
                tempPos.x = (float) Math.Round(tempPos.x);
                tempPos.y = (float) Math.Round(tempPos.y);
                tempPos.z = 0;
                instance.transform.localPosition = tempPos;
            }
        } else { Selected = null; }
    }
}
