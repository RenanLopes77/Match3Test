using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {
    private Board board;
    private Dimensions boardDim;
    private Dimensions cellDim;
    public GameObject cellGem;
    public GameObject cellGemPrefab;
    public bool isCellEmpty = false;
    public int cellIndex;
    public int columnIndex;

    public void Init(Board board, int columnIndex, int cellIndex) {
        this.board = board;
        this.boardDim = board.GetBoardDimensions();
        this.cellDim = board.GetCellDimensions();
        this.cellIndex = cellIndex;
        this.columnIndex = columnIndex;
        SetLocalPosition();
        SetName();
        SetSizeDelta();
        AddCellGem();
    }

    void SetLocalPosition() {
        transform.localPosition = new Vector3(
            0,
            cellDim.height * cellIndex - boardDim.height / 2 + cellDim.height / 2,
            0
        );
    }

    void SetName() {
        gameObject.name = $"Cell: {cellIndex}";
    }

    void SetSizeDelta() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(cellDim.width, cellDim.height);
    }

    public void AddCellGem() {
        isCellEmpty = false;
        this.cellGem = Instantiate(cellGemPrefab, Vector3.zero, Quaternion.identity, transform);
        this.cellGem.GetComponent<CellGem>().Init(this, boardDim, CellGemAnimType.Drop);
        SetGem(cellGem);
    }

    void SetGem(GameObject cellGem) {
        Gem gem = board.GetGem();
        cellGem.GetComponent<Image>().sprite = gem.sprite;
        cellGem.tag = gem.gemType.ToString();
    }

    public void FinishedToPlace() {
        board.FinishedToPlace();
    }

    public IEnumerator CheckMatchCoroutine(Axis axis) {

        while (!board.IsFinishedToPlace())
            yield return new WaitForSeconds(0.1f);

        switch (axis) {
            case Axis.Hotizontal:
                ChecMatch(DirectionEnum.LEFT, DirectionEnum.RIGHT, board.FinishedHorizontalMatch);
                break;
            case Axis.Vertical:
                ChecMatch(DirectionEnum.DOWN, DirectionEnum.UP, board.FinishedVerticalMatch);
                break;
        }

        yield return null;
    }

    void ChecMatch(DirectionEnum firstDir, DirectionEnum secondDir, Action FinishedMatch) {
        Cell firstCell = IsGemEqual(firstDir);
        if (firstCell == null) {
            Cell secondCell = IsGemEqual(secondDir);
            if (secondCell != null) {
                List<Cell> cells = secondCell.GemMatch(new List<Cell>(new Cell[] { this }), secondDir);

                if (cells.Count >= 3) {
                    board.AddMatches(cells);
                }
            }
        }

        FinishedMatch();
    }

    Cell IsGemEqual(DirectionEnum dir) {
        return board.IsGemEqual(columnIndex, cellIndex, dir, CellGem.GetGemEnum(cellGem.tag));
    }

    public List<Cell> GemMatch(List<Cell> cells, DirectionEnum direction) {
        cells.Add(this);
        Cell cell = IsGemEqual(direction);
        if (cell != null) {
            return cell.GemMatch(cells, direction);
        }
        return cells;
    }

    public void Destroy() {
        cellGem.GetComponent<CellGem>().Animate(CellGemAnimType.Destroy);
        isCellEmpty = true;
    }

    public void SetCellGem(GameObject cellGem, CellGemAnimType movementType) {
        isCellEmpty = false;
        this.cellGem = cellGem;
        this.cellGem.GetComponent<CellGem>().SetParent(transform, movementType);
    }

    public void Move(DirectionEnum direction) {
        board.GetGemsToSwap(columnIndex, cellIndex, direction);
    }
}