using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum CellGemMovementType {
    Destroy,
    Drop,
    SimpleMove,
}

public class CellGem : MonoBehaviour {

    // public float animDuration = 0.5f;
    private float animDuration = 0.8f;
    private Cell cell;

    public void Init(Cell cell, Dimensions boardDim, CellGemMovementType movementType) {
        this.cell = cell;
        transform.localPosition = new Vector3(0, boardDim.height, 0);
        Move(movementType);
    }

    public void SetParent(Transform parent, CellGemMovementType movementType) {
        transform.SetParent(parent);
        Move(movementType);
    }

    public void Move(CellGemMovementType movementType) {
        cell.SetMoving(true);
        switch (movementType) {
            case CellGemMovementType.Destroy:
                Destroy();
                break;
            case CellGemMovementType.Drop:
                Drop();
                break;
            case CellGemMovementType.SimpleMove:
                SimpleMove();
                break;
        }
    }

    private void Destroy() {
        // GetComponent<Image>().color = new Color(0, 0, 0, 0);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), animDuration).SetAutoKill().OnComplete(() => {
            // cell.CellGemDestroyed();
            cell.SetMoving(false);
            Destroy(gameObject);
        });
    }

    private void SimpleMove() {
        transform.DOMove(transform.parent.position, animDuration).SetEase(Ease.OutQuint).OnComplete(() => {
            cell.SetMoving(false);
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

    /* REMOVER REMOVER REMOVER REMOVER REMOVER REMOVER REMOVER  */
    public void SetColor(Color color) {
        GetComponent<Image>().color = color;
    }
    /* REMOVER REMOVER REMOVER REMOVER REMOVER REMOVER REMOVER  */

    public static bool IsGem(string tag) {
        return Enum.IsDefined(typeof(GemEnum), tag);
    }

    public static GemEnum GetGemEnum(string tag) {
        return (GemEnum) Enum.Parse(typeof(GemEnum), tag);
    }
}