using UnityEngine;

public class CustomTouch : Swipe {

    void Update() {
        if (Input.touchCount > 0) {
            HandleTouch();
        }
    }

    void HandleTouch() {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase) {
            case TouchPhase.Began:
                InterectionBegan(touch.position);
                break;
            case TouchPhase.Ended:
                InterectionEnded(touch.position);
                break;
        }
    }
}