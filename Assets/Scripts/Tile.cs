using UnityEngine;

namespace DefaultNamespace {

    public class Tile {
        public TileColors colors;
        public Tile(TileColors colors) {
            this.colors = colors;
        }
        public Color OnchangeColor(bool highLight) 
            => highLight ? colors.highLightColor : colors.originalColor;
        }

}