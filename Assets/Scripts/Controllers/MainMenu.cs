using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] private TMP_Text cellText = null;
    [SerializeField] private TMP_Text columnText = null;
    [SerializeField] private GameObject playButton = null;
    [SerializeField] private GameObject selectGridSize = null;
    [SerializeField] private Scene scene = null;
    private int cellAmount = 6;
    private int columnAmount = 6;

    private void Start() {
        SetCellValue();
        SetColumnValue();
        Time.timeScale = 1;
    }

    private bool CanIncrease(int value) {
        return value >= 4 && value <= 7;
    }

    public void IncreaseCell(int increase) {
        if (CanIncrease(this.cellAmount + increase)) {
            this.cellAmount += increase;
            SetCellValue();
        }
    }

    private void SetCellValue() {
        this.cellText.SetText(this.cellAmount.ToString());
        PlayerPrefs.SetInt("CellAmount", this.cellAmount);
    }

    public void IncreaseColumn(int increase) {
        if (CanIncrease(this.columnAmount + increase)) {
            this.columnAmount += increase;
            SetColumnValue();
        }
    }

    private void SetColumnValue() {
        this.columnText.SetText(this.columnAmount.ToString());
        PlayerPrefs.SetInt("ColumnAmount", this.columnAmount);
    }

    public void OnClickPlay() {
        this.playButton.SetActive(false);
        this.selectGridSize.SetActive(true);
    }

    public void OnClickStart() {
        this.scene.LoadScene(SceneNames.Game);
    }
}