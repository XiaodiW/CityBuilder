using UnityEngine;

namespace DefaultNamespace {

    [CreateAssetMenu(fileName = "ColorBlock", menuName = "Blocks", order = 0)]
    public class BlockColors : ScriptableObject {
        public Color color;
    }

}