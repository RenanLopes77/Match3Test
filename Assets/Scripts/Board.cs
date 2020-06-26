using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    private Dimensions boardDim;
    private Dimensions cellDim;
    private RectTransform rectTransform;
    public GameObject columnPrefab;
    public GameObject[, ] boardGrid;
    public List<Gem> gems;
    public Vector2 padding;
    public int cells;
    public int columns;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        boardDim = new Dimensions(Mathf.Abs(rectTransform.rect.height - padding.y), Mathf.Abs(rectTransform.rect.width - padding.x));
        cellDim = new Dimensions(Mathf.Abs(boardDim.height) / cells, Mathf.Abs(boardDim.width) / columns);
        boardGrid = new GameObject[columns, cells];
        AddColumns();
    }

    void AddColumns() {
        for (int columnIndex = 0; columnIndex < columns; columnIndex++) {
            AddColumn(columnIndex);
        }
    }

    void AddColumn(int columnIndex) {
        Column column = Instantiate(
            columnPrefab,
            Vector3.zero,
            Quaternion.identity,
            transform
        ).GetComponent<Column>();
        column.Init(this, columnIndex, cells);
    }

    public void AddCellToBoard(GameObject cell, int columnIndex, int cellIndex) {
        boardGrid[columnIndex, cellIndex] = cell;
    }

    public Dimensions GetBoardDimensions() {
        return boardDim;
    }

    public Dimensions GetCellDimensions() {
        return cellDim;
    }

    public Gem GetGem() {
        int index = Random.Range(0, gems.Count - 1);
        return gems[index];
    }

    public void SwapGems(int columnIndex, int cellIndex, DirectionEnum direction) {
        Cell currentCell = boardGrid[columnIndex, cellIndex].GetComponent<Cell>();
        Cell nextCell = null;
        switch (direction) {
            case DirectionEnum.DOWN:
                if (cellIndex > 0) {
                    Debug.Log("MoveDown");
                    nextCell = boardGrid[columnIndex, cellIndex - 1].GetComponent<Cell>();
                }
                break;
            case DirectionEnum.LEFT:
                if (columnIndex > 0) {
                    Debug.Log("MoveLeft");
                    nextCell = boardGrid[columnIndex - 1, cellIndex].GetComponent<Cell>();
                }
                break;
            case DirectionEnum.RIGHT:
                if (columnIndex < columns - 1) {
                    Debug.Log("MoveRight");
                    nextCell = boardGrid[columnIndex + 1, cellIndex].GetComponent<Cell>();
                }
                break;
            case DirectionEnum.UP:
                if (cellIndex < cells - 1) {
                    Debug.Log("MoveUp");
                    nextCell = boardGrid[columnIndex, cellIndex + 1].GetComponent<Cell>();
                }
                break;
        }
        if (currentCell != null && nextCell != null) {
            GameObject currentCellGem = currentCell.cellGem;
            GameObject nextCellGem = nextCell.cellGem;
            currentCell.SetCellGem(nextCellGem);
            nextCell.SetCellGem(currentCellGem);
        }
    }
}