using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

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
            var trans = transform;
            originalParent = trans.parent;
            originalLocalPos = trans.localPosition;
            isoGrid.RemoveBuildObject(this);
            transform.parent = isoGrid.transform;
        }

        public void OnDrag(PointerEventData eventData) {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = v3.y+0.2f;
            var trans = transform;
            trans.position = v3;
            /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit)) {
                var v3 =hit.point;
                v3.z = v3.y+0.2f;
                transform.position = v3;
            }*/
            transform.localPosition = Vector3Int.RoundToInt(trans.localPosition);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if(isoGrid.Dropable(this)) {
                isoGrid.DropBuildObject(this);
            } else {
                var trans = transform;
                trans.parent = originalParent;
                trans.localPosition = originalLocalPos;
            }
        }
    }

}