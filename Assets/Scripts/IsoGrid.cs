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
                cells[i, j] = instance;
            }
        }

        public void DropBuildObject(BuildObject buildObject) {
            foreach(var cell in SearchCells(buildObject)) cell.buildObject = buildObject;
            var theCell = cells[buildObject.position.x, buildObject.position.y];
            var bO = buildObject.transform;
            bO.parent = theCell.transform;
            bO.localPosition = Vector3.zero;
        }
        public void RemoveBuildObject(BuildObject buildObject) {
            foreach(var cell in SearchCells(buildObject)) cell.buildObject = null;
        }

        public bool Dropable(BuildObject buildObject) {
            var list = SearchCells(buildObject);
            var count = list.Count;
            return  count == buildObject.size.x * buildObject.size.y 
                    && list.Select(cell => cell.buildObject == null).FirstOrDefault();
        }

        private List<Cell> SearchCells(BuildObject buildObject) {
            var px = buildObject.position.x;
            var py = buildObject.position.y;
            var sx = buildObject.size.x;
            var sy = buildObject.size.y;
            var cellsList = new List<Cell>();
            var xTarget = math.clamp(px + sx, 0, width);
            var yTarget = math.clamp(py + sy, 0, width);
            var x = math.clamp(px, 0, width);
            var y = math.clamp(py, 0, width);
            for(var i = x; i != xTarget; i += Math.Sign(sx))
            for(var j = y; j != yTarget; j += Math.Sign(sy))
                cellsList.Add(cells[i, j]);
            return cellsList;
        }
    }

}
        