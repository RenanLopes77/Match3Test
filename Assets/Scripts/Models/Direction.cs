using UnityEngine;

public enum DirectionEnum {
    DOWN,
    LEFT,
    RIGHT,
    UP,
    DOUBLE_DOWN,
    DOUBLE_LEFT,
    DOUBLE_RIGHT,
    DOUBLE_UP,
    UPPER_LEFT,
    UPPER_RIGHT,
    LOWER_LEFT,
    LOWER_RIGHT,
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