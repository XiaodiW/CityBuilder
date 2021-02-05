using System;
using UnityEngine;

namespace DefaultNamespace {

    public class IsoGrid : MonoBehaviour {
        public Cell[,] cells;
        public int width;
        public int height;
        public Cell prefab;
        public Cell Getcell(int x, int y) => this.cells[x, y];

        public void SetCells() {
            cells = new Cell[width, height];
            for(int i = 0; i < width; i++) {
                for(int j = 0; j < height; j++) {
                    var instance = Instantiate(prefab, transform, false);
                    instance.Position = new Vector3(i, j, 0);
                    cells[i, j] = instance;
                }
            }
        }
        
    }

}
        