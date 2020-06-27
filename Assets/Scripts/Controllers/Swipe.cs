using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swipe : MonoBehaviour {
    private Cell cell = null;
    private Vector3 initialTouchPos;
    protected float minTouchDistance = 20.0f;
    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    protected void InterectionBegan(Vector3 position) {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        List<RaycastResult> results = new List<RaycastResult>();

        initialTouchPos = position;
        pointerEventData.position = initialTouchPos;
        graphicRaycaster.Raycast(pointerEventData, results);

        RaycastResult result = results.FirstOrDefault(r => CellGem.IsGem(r.gameObject.tag));
        cell = result.gameObject.transform.parent.gameObject.GetComponent<Cell>();
    }

    protected void InterectionEnded(Vector3 position) {
        if (Vector3.Distance(initialTouchPos, position) > minTouchDistance) {
            cell.Move(Direction.GetDirection(position - initialTouchPos));
            cell = null;
        }
    }
}