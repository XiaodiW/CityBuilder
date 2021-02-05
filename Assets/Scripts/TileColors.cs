using UnityEngine;

namespace DefaultNamespace {

    [CreateAssetMenu(fileName = "ColorBlock", menuName = "Blocks", order = 0)]
    public class TileColors : ScriptableObject {
        public Color originalColor;
        public Color highLightColor;
    }

}