using System;
using DG.Tweening;
using UnityEngine;

public enum CellGemMovementType {
    Drop,
    SimpleMove,
}

public class CellGem : MonoBehaviour {

    public float animDuration = 0.5f;

    public void SetParent(Transform parent, CellGemMovementType movementType) {
        transform.SetParent(parent);
        Move(movementType);
    }

    public void Move(CellGemMovementType movementType) {
        switch (movementType) {
            case CellGemMovementType.Drop:
                Drop();
                break;
            case CellGemMovementType.SimpleMove:
                SimpleMove();
                break;
        }
    }

    private void SimpleMove() {
        transform.DOMove(transform.parent.position, animDuration).SetEase(Ease.OutQuint);
    }

    private void Drop() {
        float distance = Vector3.Distance(transform.parent.position, transform.position);
        Vector3 endValue = transform.parent.position - new Vector3(0, distance * 0.1f, 0);
        transform.DOMove(endValue, animDuration)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => {
                transform.DOMove(transform.parent.position, animDuration)
                    .SetEase(Ease.OutBounce);
            });
    }

    public static bool IsGem(string tag) {
        return Enum.IsDefined(typeof(GemEnum), tag);
    }
}