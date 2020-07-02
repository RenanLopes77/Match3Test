using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    private Dimensions boardDim;
    private Dimensions cellDim;
    private RectTransform rectTransform;
    public Cell[, ] boardGrid;
    public GameObject columnPrefab;
    public List<Cell> matchedCells = new List<Cell>();
    public List<Gem> gems;
    public Vector2 padding;
    public int cells;
    public int columns;
    public int finishedHorizontalMatch = 0;
    public int finishedVerticalMatch = 0;
    public int finishedToPlace = 0;
    public int gridSize;
    public bool canSwap = false;

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
        return gems[index];
    }

    public Cell IsGemEqual(int columnIndex, int cellIndex, DirectionEnum direction, GemEnum gem) {
        Cell cell = GetCell(columnIndex, cellIndex, direction);
        if (cell != null && gem.ToString() == cell.cellGem.tag) {
            return cell;
        }
        return null;
    }

    public void FinishedToPlace() {
        finishedToPlace += 1;

        if (IsFinishedToPlace()) {
            finishedHorizontalMatch = 0;
            CheckMatch(Axis.Hotizontal);
        }
    }

    public bool IsFinishedToPlace() {
        return finishedToPlace == gridSize;
    }

    public void CheckMatch(Axis axis) {
        canSwap = false;
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < cells; j++) {
                StartCoroutine(boardGrid[i, j].CheckMatchCoroutine(axis));
            }
        }
    }

    public void AddMatches(List<Cell> matches) {
        this.matchedCells = this.matchedCells.Concat(matches).ToList();
    }

    public void FinishedHorizontalMatch() {
        finishedHorizontalMatch += 1;

        if (finishedHorizontalMatch == gridSize) {
            finishedVerticalMatch = 0;
            CheckMatch(Axis.Vertical);
        }
    }

    public void FinishedVerticalMatch() {
        finishedVerticalMatch += 1;

        if (finishedVerticalMatch == gridSize) {
            if (this.matchedCells.Count > 0) {
                StartCoroutine(DestroyGems());
            } else {
                canSwap = true;
            }
        }
    }

    IEnumerator DestroyGems() {
        List<int> columnsToDrop = new List<int>();
        this.matchedCells.ForEach(cell => {
            cell.Destroy();
            columnsToDrop.Add(cell.columnIndex);
        });
        this.matchedCells.Clear();

        yield return new WaitForSeconds(0.8f);

        StartCoroutine(DropGems(columnsToDrop.Distinct().ToList()));
    }

    IEnumerator DropGems(List<int> columnsToDrop) {
        columnsToDrop.ForEach(columnIndex => {

            List<Cell> emptyCells = new List<Cell>();

            for (int i = 0; i < cells; i++) {

                Cell currentCell = boardGrid[columnIndex, i];

                if (currentCell.isCellEmpty) {
                    finishedToPlace -= 1;
                    emptyCells.Add(currentCell);
                } else if (emptyCells.Count > 0) {
                    finishedToPlace -= 1;
                    emptyCells[0].SetCellGem(currentCell.cellGem, CellGemAnimType.SimpleMove);
                    emptyCells.RemoveAt(0);
                    currentCell.isCellEmpty = true;
                    emptyCells.Add(currentCell);
                }

            }

            if (emptyCells.Count > 0) {
                emptyCells.ForEach(emptyCell => emptyCell.AddCellGem());
                emptyCells.Clear();
            }
        });

        yield return null;
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
                if (cellIndex < cells - 1) {
                    return boardGrid[columnIndex, cellIndex + 1];
                }
                break;
        }
        return null;
    }

    public void SwapGems(int columnIndex, int cellIndex, DirectionEnum direction) {
        if (!canSwap) return;

        Cell currentCell = boardGrid[columnIndex, cellIndex];
        Cell nextCell = GetCell(columnIndex, cellIndex, direction);
        if (currentCell != null && nextCell != null) {
            GameObject currentCellGem = currentCell.cellGem;
            GameObject nextCellGem = nextCell.cellGem;
            finishedToPlace -= 2;
            currentCell.SetCellGem(nextCellGem, CellGemAnimType.SimpleMove);
            nextCell.SetCellGem(currentCellGem, CellGemAnimType.SimpleMove);
            canSwap = false;
        }
    }
}