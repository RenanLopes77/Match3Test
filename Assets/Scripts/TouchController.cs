using UnityEngine;

public class TouchController : SwipeController {

    void Update() {
        if (Input.touchCount > 0) {
            HandleTouch();
        }
    }

    void HandleTouch() {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase) {
            case TouchPhase.Began:
                InterectionBegan(new Vector3(touch.position.x, touch.position.y, 0));
                break;
            case TouchPhase.Ended:
                InterectionEnded(new Vector3(touch.position.x, touch.position.y, 0));
                break;
        }
    }
}