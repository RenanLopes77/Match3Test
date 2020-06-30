using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyButtons;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {
    private Board board;
    private Dimensions boardDim;
    private Dimensions cellDim;
    public GameObject cellGem;
    public GameObject cellGemPrefab;
    public int cellIndex;
    public int columnIndex;
    public bool isCellEmpty = false;
    public bool isMoving = false;
    public bool isMatched = false;

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
        this.cellGem.GetComponent<CellGem>().Init(this, boardDim, CellGemMovementType.Drop);
        SetGem(cellGem);
    }

    void SetGem(GameObject cellGem) {
        Gem gem = board.GetGem();
        cellGem.GetComponent<Image>().sprite = gem.sprite;
        cellGem.tag = gem.gemType.ToString();
    }

    public void FinishedToPlace() {
        board.FinishedToPlace();
        SetMoving(false);
    }

    public IEnumerator CheckMatchCoroutine() {

        isMatched = false;

        while (board.finishedToPlace != board.gridSize || isMoving)
            yield return new WaitForSeconds(0.1f);

        cellGem.GetComponent<CellGem>().SetColor(Color.red);

        yield return new WaitForSeconds(2f);

        cellGem.GetComponent<CellGem>().SetColor(Color.black);

        Cell leftCell = IsGemEqual(DirectionEnum.LEFT);

        if (leftCell == null) {
            Cell rightCell = IsGemEqual(DirectionEnum.RIGHT);
            if (rightCell == null) {

                // Cell downCell = IsGemEqual(DirectionEnum.DOWN);

                // if (downCell == null) {
                //     Cell upCell = IsGemEqual(DirectionEnum.UP);

                //     if (upCell == null) {
                //         Debug.Log($"END MATCH");
                //         board.AddMatches(new List<Cell>(), 1);
                //     } else {
                //         Debug.Log($"END MATCH");
                //         isMatched = true;
                //         upCell.WeAreEqual(new List<Cell>(new Cell[] { this }), DirectionEnum.UP);
                //     }
                // }

                cellGem.GetComponent<CellGem>().SetColor(Color.yellow);
                board.AddMatches(new List<Cell>(), 1);

            } else {
                isMatched = true;
                List<Cell> cells = rightCell.WeAreEqual(new List<Cell>(new Cell[] { this }), DirectionEnum.RIGHT);

                cellGem.GetComponent<CellGem>().SetColor(Color.blue);

                if (cells.Count >= 3) {
                    board.AddMatches(cells, cells.Count);
                } else {
                    board.AddMatches(new List<Cell>(), cells.Count);
                }
            }
        } else {
            cellGem.GetComponent<CellGem>().SetColor(Color.green);
        }

        yield return null;
    }

    Cell IsGemEqual(DirectionEnum dir) {
        return board.IsGemEqual(columnIndex, cellIndex, dir, CellGem.GetGemEnum(cellGem.tag));
    }

    // CHANGE NAME
    public List<Cell> WeAreEqual(List<Cell> cells, DirectionEnum direction) {
        isMatched = true;
        cells.Add(this);
        Cell cell = IsGemEqual(direction);

        if (cell != null && !cell.isMatched) {
            return cell.WeAreEqual(cells, direction);
        }
        return cells;
    }

    public void Destroy() {
        cellGem.GetComponent<CellGem>().Move(CellGemMovementType.Destroy);
        // this.cellGem = null;
        isCellEmpty = true;
    }

    public void SetCellGem(GameObject cellGem, CellGemMovementType movementType) {
        isCellEmpty = false;
        this.cellGem = cellGem;
        this.cellGem.GetComponent<CellGem>().SetParent(transform, movementType);
    }

    public void Move(DirectionEnum direction) {
        board.SwapGems(columnIndex, cellIndex, direction);
    }

    public void SetMoving(bool moving) {
        this.isMoving = moving;
    }
}