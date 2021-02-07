using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace {

    public class IsoGrid : MonoBehaviour {
        public Cell[,] cells;
        public int width;
        public int height;
        public Cell prefab;
        public Cell Getcell(int x, int y) => this.cells[x, y];

        private void Awake() {
            PlantCells();
        }

        public void PlantCells() {
            cells = new Cell[width, height];
            for(var i = 0; i < width; i++)
            for(var j = 0; j < height; j++) {
                var instance = Instantiate(prefab, transform, false);
                instance.Position = new Vector3(i, j, 0);
                instance.name = $"Cell({i},{j})";
                cells[i, j] = instance;
            }
        }

        public void DropBuildObject(BuildObject buildObject) {
            var list = SearchCells(buildObject);
            foreach(var cell in list) {
                cell.buildObject = buildObject;
            }
        }
        public void RemoveBuildObject(BuildObject buildObject) {
            foreach(var cell in SearchCells(buildObject)) cell.buildObject = null;
        }

        public bool Dropable(BuildObject buildObject) {
            var list = SearchCells(buildObject);
            var count = list.Count;
            var i = list.Count(cell => cell.buildObject == null);
            return count == buildObject.size.x * buildObject.size.y &&
                   list.Count(cell => cell.buildObject == null) == count;
        }

        private List<Cell> SearchCells(BuildObject buildObject) {
            var px = (int)buildObject.transform.localPosition.x;
            var py = (int)buildObject.transform.localPosition.y;
            var sx = buildObject.size.x;
            var sy = buildObject.size.y;
            var cellsList = new List<Cell>();
            for(var i = px; i != px + sx; i += Math.Sign(sx))
            for(var j = py; j != py + sy; j += Math.Sign(sy))
                if(Enumerable.Range(0, width).Contains(i) && Enumerable.Range(0, height).Contains(j))
                    cellsList.Add(cells[i, j]);
            return cellsList;
        }
    }

}
        