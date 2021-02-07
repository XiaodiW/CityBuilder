using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace {

    public class BuildObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Vector2Int size;
        private Transform originalParent;
        private Vector3 originalLocalPos;
        private IsoGrid isoGrid;
        private Renderer renderer;

        private void Awake() {
            renderer = gameObject.GetComponent<Renderer>();
            isoGrid = GetComponentInParent<IsoGrid>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            originalParent = transform.parent;
            originalLocalPos = transform.localPosition;
            isoGrid.RemoveBuildObject(this);
            transform.parent = isoGrid.transform;
        }

        public void OnDrag(PointerEventData eventData) {
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = v3.y+0.2f;
            transform.position = v3;
            /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit)) {
                var v3 =hit.point;
                v3.z = v3.y+0.2f;
                transform.position = v3;
            }*/
            transform.localPosition = Vector3Int.RoundToInt(transform.localPosition);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if(isoGrid.Dropable(this)) {
                isoGrid.DropBuildObject(this);
            } else {
                transform.parent = originalParent;
                transform.localPosition = originalLocalPos;
            }
        }
    }

}