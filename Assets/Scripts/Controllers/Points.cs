using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour {
    [SerializeField] private GameObject _Dynamic = null;
    [SerializeField] private GameObject pointsPrefab = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_Text currentPoints = null;
    [SerializeField] private TMP_Text goalPoints = null;
    [SerializeField] private int multiplier = 20;
    [SerializeField] private int points = 0;

    private void Start() {
        SetGoalValues();
    }

    public void AddPoints(int cellsDestroyed, List<Cell> cells) {
        int points = (int) (multiplier * cellsDestroyed * cells[0].cellGem.GetPointsMultiplier());
        this.points += points;
        UpdatePoints();
        StartCoroutine(DisplayPoints(GetCommonPosition(cells), points));
    }

    void UpdatePoints() {
        this.slider.value = this.points;
        this.currentPoints.SetText(FormatIntToString(this.points));
    }

    void SetGoalValues() {
        int goalValue = GetGoalValue();
        this.slider.minValue = this.points;
        this.slider.maxValue = goalValue;
        this.goalPoints.SetText(FormatIntToString(goalValue));
        this.currentPoints.SetText(FormatIntToString(this.points));
    }

    string FormatIntToString(int number) {
        return number.ToString("n0");
    }

    int GetGoalValue() {
        return 10000;
    }

    Vector3 GetCommonPosition(List<Cell> cells) {
        return Vector3.Lerp(cells[0].transform.position, cells[cells.Count - 1].transform.position, 0.5f);
    }

    IEnumerator DisplayPoints(Vector3 pos, int points) {

        GameObject pointsText = Instantiate(pointsPrefab, pos, Quaternion.identity, this._Dynamic.transform);
        pointsText.GetComponent<TMP_Text>().SetText(FormatIntToString(points));

        yield return new WaitForSeconds(0.5f);

        Destroy(pointsText);
    }
}