using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour,IPointerClickHandler {
    private IMySelectable selected;
    [HideInInspector]public GameObject originalParent;
    private bool isSpawn;
    public Button spawanTrigger;
    public GameObject prefab;
    public List<BlockColors> colorList;
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
                SpriteRenderer b =new SpriteRenderer();
                var tempArr = instance.GetComponentsInChildren<SpriteRenderer>();
                foreach(var temp in tempArr)
                    if(temp.transform.parent == instance.transform)
                        b = temp;
                b.color = colorList[Random.Range(0, colorList.Count)].color;
            }
        } else { Selected = null; }
    }
}
