using UnityEngine;

namespace DefaultNamespace {

    public class Cell : MonoBehaviour {
        private Vector3 position;
        public BuildObject buildObject;
        public Vector3 Position {
            get => position;
            set {
                position = value;
                transform.localPosition = position;
            }
        }
    }

}