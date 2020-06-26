using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {
    private Cell cell = null;
    private Vector2 initialTouchPos;
    private float minTouchDistance = 20.0f;
    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    void Update() {
        if (Input.touchCount > 0) {
            HandleTouch();
        }
    }

    void HandleTouch() {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase) {
            case TouchPhase.Began:
                OnTouchBegan(touch);
                break;
            case TouchPhase.Ended:
                OnTouchEnded(touch);
                break;
        }
    }

    void OnTouchBegan(Touch touch) {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        List<RaycastResult> results = new List<RaycastResult>();

        initialTouchPos = touch.position;
        pointerEventData.position = initialTouchPos;
        graphicRaycaster.Raycast(pointerEventData, results);

        RaycastResult result = results.FirstOrDefault(r => CellGem.IsGem(r.gameObject.tag));
        cell = result.gameObject.transform.parent.gameObject.GetComponent<Cell>();
    }

    void OnTouchEnded(Touch touch) {
        if (Vector3.Distance(initialTouchPos, touch.position) > minTouchDistance) {
            cell.Move(GetDirection(touch.position - initialTouchPos));
            cell = null;
        }
    }

    Direction GetDirection(Vector2 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                return Direction.RIGHT;
            } else {
                return Direction.LEFT;
            }
        } else {
            if (direction.y > 0) {
                return Direction.UP;
            } else {
                return Direction.DOWN;
            }
        }
    }
}