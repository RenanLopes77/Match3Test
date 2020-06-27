using UnityEngine;

public class Mouse : Swipe {

    void Update() {
        HandleInputMouse();
    }

    void HandleInputMouse() {
        if (Input.GetMouseButtonDown(0)) {
            InterectionBegan(Input.mousePosition);
        } else if (Input.GetMouseButtonUp(0)) {
            InterectionEnded(Input.mousePosition);
        }
    }
}