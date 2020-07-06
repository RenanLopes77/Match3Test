using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour {
    [SerializeField] private Audio audio = null;
    [SerializeField] private GameObject _Dynamic = null;
    [SerializeField] private GameObject pointsPrefab = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_Text currentPointsText = null;
    [SerializeField] private TMP_Text goalPointsText = null;
    [SerializeField] private int multiplier = 20;
    [SerializeField] private int points = 0;
    private int goalPoints = 0;
    private int stepPoints = 0;
    private List<Action> onReachGoalPointsCallbacks = new List<Action>();

    private void Start() {
        SetGoalValues();
    }

    public void AddPoints(int cellsDestroyed, List<Cell> cells) {
        int points = (int) (multiplier * cellsDestroyed * cells[0].cellGem.GetPointsMultiplier());
        this.points += points;
        UpdatePoints();
        StartCoroutine(DisplayPoints(GetCommonPosition(cells), points));
        OnReachGoalPoints();
    }

    public void AddOnReachGoalPointsCallBack(Action action) {
        this.onReachGoalPointsCallbacks.Add(action);
    }

    void UpdatePoints() {
        this.slider.value = this.points;
        this.currentPointsText.SetText(FormatIntToString(this.points));
    }

    void SetGoalValues() {
        SetGoalValue();
        this.slider.minValue = this.points;
        this.slider.maxValue = this.goalPoints;
        this.goalPointsText.SetText(FormatIntToString(this.goalPoints));
        this.currentPointsText.SetText(FormatIntToString(this.points));
    }

    void OnReachGoalPoints() {
        if (this.points >= this.goalPoints) {
            this.audio.PlayclearSound();
            SetGoalValues();
            this.onReachGoalPointsCallbacks.ForEach(callBack => {
                callBack();
            });
        }
    }

    string FormatIntToString(int number) {
        return number.ToString("n0");
    }

    void SetGoalValue() {
        this.stepPoints += 5000;
        this.goalPoints += this.stepPoints;
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