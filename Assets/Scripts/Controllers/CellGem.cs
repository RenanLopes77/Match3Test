using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum CellGemAnimType {
    Destroy,
    Drop,
    SimpleMove,
}

public class CellGem : MonoBehaviour {

    // public float animDuration = 0.5f;
    private float animDuration = 0.8f;
    private Cell cell;

    public void Init(Cell cell, Dimensions boardDim, CellGemAnimType movementType) {
        this.cell = cell;
        transform.localPosition = new Vector3(0, boardDim.height, 0);
        Animate(movementType);
    }

    public void SetParent(Transform parent, CellGemAnimType movementType) {
        transform.SetParent(parent);
        Animate(movementType);
    }

    public void Animate(CellGemAnimType movementType) {
        switch (movementType) {
            case CellGemAnimType.Destroy:
                Destroy();
                break;
            case CellGemAnimType.Drop:
                Drop();
                break;
            case CellGemAnimType.SimpleMove:
                SimpleMove();
                break;
        }
    }

    private void Destroy() {
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), animDuration).SetAutoKill().OnComplete(() => {
            Destroy(gameObject);
        });
    }

    private void SimpleMove() {
        transform.DOMove(transform.parent.position, animDuration).SetEase(Ease.OutQuint).OnComplete(() => {
            cell.FinishedToPlace();
        });;
    }

    private void Drop() {
        float distance = Vector3.Distance(transform.parent.position, transform.position);
        Vector3 endValue = transform.parent.position - new Vector3(0, distance * 0.05f, 0);
        transform.DOMove(endValue, animDuration)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => {
                transform.DOMove(transform.parent.position, animDuration)
                    .SetEase(Ease.OutBounce)
                    .OnComplete(() => {
                        cell.FinishedToPlace();
                    });
            });
    }

    public static bool IsGem(string tag) {
        return Enum.IsDefined(typeof(GemEnum), tag);
    }

    public static GemEnum GetGemEnum(string tag) {
        return (GemEnum) Enum.Parse(typeof(GemEnum), tag);
    }
}