﻿using System.Collections;
using System.Collections.Generic;
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

    void AddCellGem() {
        cellGem = Instantiate(cellGemPrefab, Vector3.zero, Quaternion.identity, transform);
        cellGem.transform.localPosition = Vector3.zero;
        SetGem(cellGem);
    }

    void SetGem(GameObject cellGem) {
        Gem gem = board.GetGem();
        cellGem.GetComponent<Image>().sprite = gem.sprite;
        cellGem.tag = gem.gemType.ToString();
    }

    public void SetCellGem(GameObject cellGem) {
        this.cellGem = cellGem;
        this.cellGem.GetComponent<CellGem>().SetParent(transform);
    }

    [Button] void MoveDown() {
        board.SwapGems(columnIndex, cellIndex, Direction.DOWN);
    }

    [Button] void MoveLeft() {
        board.SwapGems(columnIndex, cellIndex, Direction.LEFT);
    }

    [Button] void MoveRight() {
        board.SwapGems(columnIndex, cellIndex, Direction.RIGHT);
    }

    [Button] void MoveUp() {
        board.SwapGems(columnIndex, cellIndex, Direction.UP);
    }
}