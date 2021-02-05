using UnityEngine;

namespace DefaultNamespace {

    public class Cell : MonoBehaviour {
        private Vector3 position;
        
        public Vector3 Position {
            get => position;
            set {
                position = value;
                transform.localPosition = position;
            }
        }
    }

}