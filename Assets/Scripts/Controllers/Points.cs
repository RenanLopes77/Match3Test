using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour {
    [SerializeField] private int points = 0;
    [SerializeField] private int multiplier = 20;
    [SerializeField] private TMP_Text currentPoints = null;
    [SerializeField] private TMP_Text goalPoints = null;
    [SerializeField] private Slider slider = null;

    private void Start() {
        SetGoalValues();
    }

    public void AddPoints(int cellsDestroyed) {
        this.points += multiplier * cellsDestroyed;
        UpdatePoints();
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
        return 1000;
    }
}