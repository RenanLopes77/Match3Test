using UnityEngine;

public enum DirectionEnum {
    DOWN,
    LEFT,
    RIGHT,
    UP,
}

public enum Axis {
    Hotizontal,
    Vertical
}

public class Direction {
    public static DirectionEnum GetDirection(Vector3 direction) {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                return DirectionEnum.RIGHT;
            } else {
                return DirectionEnum.LEFT;
            }
        } else {
            if (direction.y > 0) {
                return DirectionEnum.UP;
            } else {
                return DirectionEnum.DOWN;
            }
        }
    }
}