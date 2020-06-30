using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    private Dimensions boardDim;
    private Dimensions cellDim;
    private RectTransform rectTransform;
    public GameObject columnPrefab;
    public Cell[, ] boardGrid;
    public List<Gem> gems;
    public Vector2 padding;
    public int cells;
    public int columns;

    public int gridSize;

    // *************************************

    public List<Cell> matchedCells = new List<Cell>();
    public int finishedToPlace = 0;
    public int finishedToMatch = 0;

    // *************************************

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        boardDim = new Dimensions(Mathf.Abs(rectTransform.rect.height - padding.y), Mathf.Abs(rectTransform.rect.width - padding.x));
        cellDim = new Dimensions(Mathf.Abs(boardDim.height) / cells, Mathf.Abs(boardDim.width) / columns);
        boardGrid = new Cell[columns, cells];
        gridSize = columns * cells;
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

    public Dimensions GetBoardDimensions() {
        return boardDim;
    }

    public Dimensions GetCellDimensions() {
        return cellDim;
    }

    public void AddCellToBoard(Cell cell, int columnIndex, int cellIndex) {
        boardGrid[columnIndex, cellIndex] = cell;
    }

    public Gem GetGem() {
        int index = UnityEngine.Random.Range(0, gems.Count);
        // int index = UnityEngine.Random.Range(0, 2);
        return gems[index];
    }

    public Cell IsGemEqual(int columnIndex, int cellIndex, DirectionEnum direction, GemEnum gem) {
        Cell cell = GetCell(columnIndex, cellIndex, direction);
        if (cell != null && gem.ToString() == cell.cellGem.tag) {
            return cell;
        }
        return null;
    }

    public void AddMatches(List<Cell> matches, int amount) {
        this.matchedCells = this.matchedCells.Concat(matches).ToList();

        finishedToMatch += amount;

        if (finishedToMatch == gridSize) {
            Debug.Log($"22 FinishedToCheckMatches");
            if (this.matchedCells.Count > 0) {
                finishedToMatch = 0;
                StartCoroutine(DestroyGems());
            }
        }
    }

    IEnumerator DestroyGems() {

        Debug.Log($"333 ToDestroy");

        List<int> toDrop = new List<int>();
        this.matchedCells.ForEach(cell => {
            finishedToPlace -= 1;
            cell.Destroy();
            toDrop.Add(cell.columnIndex);
        });
        this.matchedCells.Clear();

        Debug.Log($"333 FinishedToDestroy");

        yield return new WaitForSeconds(0.8f);

        Debug.Log($"4444 ToDrop");

        toDrop = toDrop.Distinct().ToList();
        toDrop.ForEach(td => {
            DropGems(td);
        });

        Debug.Log($"4444 FinishedToDrop");
    }

    public void DropGems(int columnIndex) {
        List<Cell> emptyCells = new List<Cell>();

        for (int i = 0; i < cells; i++) {

            Cell currentCell = boardGrid[columnIndex, i];

            if (currentCell.isCellEmpty) {
                emptyCells.Add(currentCell);
            } else if (emptyCells.Count > 0) {
                emptyCells[0].SetCellGem(currentCell.cellGem, CellGemMovementType.SimpleMove);
                emptyCells.RemoveAt(0);
                currentCell.isCellEmpty = true;
                emptyCells.Add(currentCell);
            }

        }

        if (emptyCells.Count > 0) {
            emptyCells.ForEach(emptyCell => {
                emptyCell.AddCellGem();
            });
            emptyCells.Clear();
        }
    }

    public void FinishedToPlace() {
        finishedToPlace += 1;

        if (finishedToPlace == gridSize) {
            Debug.Log($"1 FinishedToPlace");
            CheckAllAgain();
        }
    }

    public void CheckAllAgain() {
        Debug.Log($"22 CheckMatches");
        finishedToMatch = 0;

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < cells; j++) {
                StartCoroutine(boardGrid[i, j].CheckMatchCoroutine());
            }
        }
    }

    public bool IsLastColumnCell(int cellIndex) {
        if (cellIndex < cells - 1) {
            return false;
        }
        return true;
    }

    public Cell GetCell(int columnIndex, int cellIndex, DirectionEnum direction) {
        switch (direction) {
            case DirectionEnum.DOWN:
                if (cellIndex > 0) {
                    return boardGrid[columnIndex, cellIndex - 1];
                }
                break;
            case DirectionEnum.LEFT:
                if (columnIndex > 0) {
                    return boardGrid[columnIndex - 1, cellIndex];
                }
                break;
            case DirectionEnum.RIGHT:
                if (columnIndex < columns - 1) {
                    return boardGrid[columnIndex + 1, cellIndex];
                }
                break;
            case DirectionEnum.UP:
                if (!IsLastColumnCell(cellIndex)) {
                    return boardGrid[columnIndex, cellIndex + 1];
                }
                break;
        }
        return null;
    }

    public void SwapGems(int columnIndex, int cellIndex, DirectionEnum direction) {
        Cell currentCell = boardGrid[columnIndex, cellIndex];
        Cell nextCell = GetCell(columnIndex, cellIndex, direction);
        if (currentCell != null && nextCell != null) {
            GameObject currentCellGem = currentCell.cellGem;
            GameObject nextCellGem = nextCell.cellGem;
            currentCell.SetCellGem(nextCellGem, CellGemMovementType.SimpleMove);
            nextCell.SetCellGem(currentCellGem, CellGemMovementType.SimpleMove);
        }

        CheckAllAgain();
    }
}