using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

    private Board board;
    private Dimensions boardDim;
    private Dimensions cellDim;
    public GameObject cellPrefab;
    public int columnIndex;

    public void Init(Board board, int columnIndex, int cells) {
        this.board = board;
        this.boardDim = board.GetBoardDimensions();
        this.cellDim = board.GetCellDimensions();
        this.columnIndex = columnIndex;
        SetLocalPosition();
        SetName();
        SetSizeDelta();
        AddCells(cells);
    }

    void SetLocalPosition() {
        transform.localPosition = new Vector3(cellDim.width * columnIndex - boardDim.width / 2 + cellDim.width / 2, 0, 0);
    }
    void SetName() {
        gameObject.name = $"Column: {columnIndex}";
    }
    void SetSizeDelta() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(cellDim.width, rectTransform.sizeDelta.y);
    }

    void AddCells(int cells) {
        for (int cellIndex = 0; cellIndex < cells; cellIndex++) {
            Addcell(cellIndex);
        }
    }

    void Addcell(int cellIndex) {
        Cell cell = Instantiate(cellPrefab,
            Vector3.zero,
            Quaternion.identity,
            transform
        ).GetComponent<Cell>();
        cell.Init(board, columnIndex, cellIndex);
        board.AddCellToBoard(cell.gameObject, columnIndex, cellIndex);
    }
}