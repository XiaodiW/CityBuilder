using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

namespace DefaultNamespace {

    public class BuildObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Vector2Int size;
        private Transform originalParent;
        private Vector3 originalLocalPos;
        private IsoGrid isoGrid;
        private SpriteRenderer[] renderers;

        private void Awake() {
            renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            isoGrid = GetComponentInParent<IsoGrid>();
        }

        public void OnBeginDrag(PointerEventData eventData) {
            var trans = transform;
            originalParent = trans.parent;
            originalLocalPos = trans.localPosition;
            isoGrid.RemoveBuildObject(this);
            transform.parent = isoGrid.transform;
            ChangeRendererColor(true);
        }

        private void ChangeRendererColor(bool halfTransparent) {
            var transparent = renderers[0].color;
            transparent.a = halfTransparent? 0.5f: 1f;
            foreach(var render in renderers) render.color = transparent;
        }

        public void OnDrag(PointerEventData eventData) {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v3.z = v3.y+0.2f;
            var trans = transform;
            trans.position = v3;
            transform.localPosition = Vector3Int.RoundToInt(trans.localPosition);
            /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hit)) {
                var v3 = hit.point;
                v3.z = v3.y + 0.2f;
                transform.position = v3;
            }*/
        }

        public void OnEndDrag(PointerEventData eventData) {
            if(isoGrid.Dropable(this)) {
                isoGrid.DropBuildObject(this);
            } else {
                var trans = transform;
                trans.parent = originalParent;
                trans.localPosition = originalLocalPos;
            }
            ChangeRendererColor(false);
        }
    }

}