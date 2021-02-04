using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour,IPointerClickHandler {
    private IMySelectable selectedWall;
    [HideInInspector]public GameObject originalParent;
    private bool isSpawnOn;
    public Button modeButton;
    public GameObject prefab;
    public List<BlockColors> PresetColors;
    private GameObject isoMetricGrid;
    private Text modeButtonText;
    [HideInInspector] public bool targetCanSpawn;

    private void Start() {
        isoMetricGrid = GameObject.Find("IsoMetricGrid");
        modeButtonText = modeButton.GetComponentInChildren<Text>();
        modeButtonText.text = isSpawnOn ? "Spawn Mode" : "Normal Mode";
    }

    public IMySelectable Selected {
        set {
            selectedWall?.RemoveSelect();
            selectedWall = value;
        } 
    }

    public void SpawnTrigger() {
        isSpawnOn = !isSpawnOn;
        modeButtonText.text = isSpawnOn ? "Spawn Mode" : "Normal Mode";
    }

    public void OnPointerClick(PointerEventData eventData) {
        if(isSpawnOn) {
            if(targetCanSpawn) DoSpawn(eventData);
        } else { Selected = null; }
    }
    
    void DoSpawn(PointerEventData pointerEventData) {
        var mousePosition = pointerEventData.pointerCurrentRaycast.worldPosition;
        var instance = Instantiate(prefab, isoMetricGrid.transform);
        instance.transform.position = mousePosition;
        var tempPos = instance.transform.localPosition;
        tempPos.x = (float) Math.Round(tempPos.x);
        tempPos.y = (float) Math.Round(tempPos.y);
        tempPos.z = 0;
        instance.transform.localPosition = tempPos;
        SpriteRenderer b = new SpriteRenderer();
        var tempArr = instance.GetComponentsInChildren<SpriteRenderer>();
        foreach(var temp in tempArr)
            if(temp.transform.parent == instance.transform)
                b = temp;
        var colorSelected = PresetColors[Random.Range(0, PresetColors.Count)];
        b.color = colorSelected.color;
        b.gameObject.name = colorSelected.name;
    }
}
