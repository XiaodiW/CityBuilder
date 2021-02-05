using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace {

    public class BuildObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Vector2Int position;
        public Vector2Int size;
        private Transform myOriginalParent;
        private IsoGrid isoGrid;
        private Renderer renderer;

        private void Awake() {
            renderer = gameObject.GetComponent<Renderer>();
            isoGrid = GetComponentInParent<IsoGrid>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            myOriginalParent = transform.parent;
            isoGrid.RemoveBuildObject(this);
            transform.parent = null;
        }

        public void OnDrag(PointerEventData eventData) {
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = v3.y+0.2f;
            transform.position = v3;
            /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit,LayerMask.GetMask("Ground"))) { //
                var v3 =hit.point;
                v3.z = v3.y+0.2f;
                transform.position = v3;
            }*/
            // Debug.Log($"Position {transform.position}");
        }

        public void OnEndDrag(PointerEventData eventData) {
            transform.parent = isoGrid.transform;
            position = Vector2Int.RoundToInt(transform.localPosition);
            if(isoGrid.Dropable(this)) {
                isoGrid.DropBuildObject(this);
            } else {
                transform.parent = myOriginalParent;
                transform.localPosition = Vector3.zero;
            }
        }
    }

}