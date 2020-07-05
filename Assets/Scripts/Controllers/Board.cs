using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour {
    [SerializeField] private GameObject columnPrefab = null;
    [SerializeField] private List<Gem> gems = new List<Gem>();
    [SerializeField] private Points points = null;
    [SerializeField] private Vector2 padding = Vector2.zero;
    [SerializeField] private bool canSwap = false;
    [SerializeField] private int cells = 0;
    [SerializeField] private int columns = 0;
    [SerializeField] private int finishedHorizontalMatch = 0;
    [SerializeField] private int finishedToPlace = 0;
    [SerializeField] private int finishedVerticalMatch = 0;
    [SerializeField] private int gridSize = 0;
    [SerializeField] private int possibleMovements = 0;
    [SerializeField] private int timesShuffled = 0;
    private Cell[, ] boardGrid;
    private Coroutine showPossibleMoventCoroutine = null;
    private Dimensions boardDim;
    private Dimensions cellDim;
    private LastSwap lastSwap = null;
    private List<Cell> matchedCells = new List<Cell>();
    private List<Cell> possibleMovementsCells = new List<Cell>();
    private RectTransform rectTransform;

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

    public Gem GetGemByIndex(int index) {
        return gems[index];
    }

    public Cell IsGemEqual(int columnIndex, int cellIndex, DirectionEnum direction, GemEnum gem) {
        Cell cell = GetCell(columnIndex, cellIndex, direction);
        if (cell != null && gem.ToString() == cell.cellGem.tag) {
            return cell;
        }
        return null;
    }

    public void AddPossibleMovement(List<Cell> cells) {
        this.possibleMovements += 1;
        this.timesShuffled = 0;

        if (this.possibleMovements == 1) {
            this.possibleMovementsCells = this.possibleMovementsCells.Concat(cells).ToList();
        }
    }

    public bool HasPossibleMovement() {
        return possibleMovements > 0;
    }

    void CleanPossibleMovement() {
        this.possibleMovements = 0;
        this.possibleMovementsCells.Clear();
    }

    public bool HasMatches() {
        return this.matchedCells.Count > 0;
    }

    public void FinishedToPlace() {
        finishedToPlace += 1;

        if (IsFinishedToPlace()) {
            finishedHorizontalMatch = 0;
            CleanPossibleMovement();
            CheckMatch(Axis.Hotizontal);
        }
    }

    public bool IsFinishedToPlace() {
        return finishedToPlace == gridSize;
    }

    public void CheckMatch(Axis axis) {
        SetCanSwap(false);
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < cells; j++) {
                StartCoroutine(boardGrid[i, j].CheckMatchCoroutine(axis));
            }
        }
    }

    public void AddMatches(List<Cell> matches) {
        this.matchedCells = this.matchedCells.Concat(matches).ToList();
        this.points.AddPoints(matches.Count, matches);
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
                this.timesShuffled = 0;
                lastSwap = null;
                StartCoroutine(DestroyGems());
            } else if (lastSwap != null) {
                UndoSwap();
            } else if (!HasPossibleMovement()) {
                ShuffleGems();
            } else {
                SetCanSwap(true);
                showPossibleMoventCoroutine = StartCoroutine(ShowPossibleMovent());
            }
        }
    }

    IEnumerator ShowPossibleMovent() {
        yield return new WaitForSeconds(15);

        while (true) {
            yield return new WaitForSeconds(1f);
            possibleMovementsCells.ForEach(cell => cell.Punch());
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
                return GetCellFromGrid(columnIndex, cellIndex - 1);
            case DirectionEnum.LEFT:
                return GetCellFromGrid(columnIndex - 1, cellIndex);
            case DirectionEnum.RIGHT:
                return GetCellFromGrid(columnIndex + 1, cellIndex);
            case DirectionEnum.UP:
                return GetCellFromGrid(columnIndex, cellIndex + 1);
            case DirectionEnum.DOUBLE_DOWN:
                return GetCellFromGrid(columnIndex, cellIndex - 2);
            case DirectionEnum.DOUBLE_LEFT:
                return GetCellFromGrid(columnIndex - 2, cellIndex);
            case DirectionEnum.DOUBLE_RIGHT:
                return GetCellFromGrid(columnIndex + 2, cellIndex);
            case DirectionEnum.DOUBLE_UP:
                return GetCellFromGrid(columnIndex, cellIndex + 2);
            case DirectionEnum.UPPER_LEFT:
                return GetCellFromGrid(columnIndex - 1, cellIndex + 1);
            case DirectionEnum.UPPER_RIGHT:
                return GetCellFromGrid(columnIndex + 1, cellIndex + 1);
            case DirectionEnum.LOWER_LEFT:
                return GetCellFromGrid(columnIndex - 1, cellIndex - 1);
            case DirectionEnum.LOWER_RIGHT:
                return GetCellFromGrid(columnIndex + 1, cellIndex - 1);
        }

        return null;
    }

    Cell GetCellFromGrid(int columnIndex, int cellIndex) {
        if (columnIndex < 0 ||
            columnIndex >= columns ||
            cellIndex < 0 ||
            cellIndex >= cells) return null;

        return boardGrid[columnIndex, cellIndex];
    }

    public void SetCanSwap(bool canSwap) {
        this.canSwap = canSwap;
    }

    void UndoSwap() {
        SwapGems(lastSwap.cellOne, lastSwap.cellTwo);
        lastSwap = null;
    }

    public void GetGemsToSwap(int columnIndex, int cellIndex, DirectionEnum direction) {
        if (!canSwap) return;

        Cell currentCell = boardGrid[columnIndex, cellIndex];
        Cell nextCell = GetCell(columnIndex, cellIndex, direction);
        if (currentCell != null && nextCell != null) {
            SwapGems(currentCell, nextCell);
            lastSwap = new LastSwap(direction, currentCell, nextCell);
        }
    }

    void SwapGems(Cell cellOne, Cell cellTwo) {
        if (showPossibleMoventCoroutine != null) StopCoroutine(showPossibleMoventCoroutine);
        CellGem CellOneGem = cellOne.cellGem;
        CellGem CellTwoGem = cellTwo.cellGem;
        finishedToPlace -= 2;
        cellOne.SetCellGem(CellTwoGem, CellGemAnimType.SimpleMove);
        cellTwo.SetCellGem(CellOneGem, CellGemAnimType.SimpleMove);
        SetCanSwap(false);
    }

    void ShuffleGems() {
        timesShuffled += 1;

        if (timesShuffled < 10) {
            for (int i = 0; i < gridSize / 2; i++) {
                SwapGems(
                    boardGrid[Random.Range(0, columns), Random.Range(0, cells)],
                    boardGrid[Random.Range(0, columns), Random.Range(0, cells)]
                );
            }
        } else {
            timesShuffled = 0;
            DestroyAllGems();
        }
    }

    void DestroyAllGems() {
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < cells; j++) {
                this.matchedCells.Add(boardGrid[i, j]);
            }
        }
        StartCoroutine(DestroyGems());
    }
}