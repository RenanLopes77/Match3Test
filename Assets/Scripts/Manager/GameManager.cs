﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Board board = null;
    [SerializeField] private Timer timer = null;
    [SerializeField] private Points points = null;
    public int round = 1;

    private void Start() {
        points.AddOnReachGoalPointsCallBack(this.OnReachGoalPoints);
        timer.AddCountdownFinishCallBack(this.StopGame);
    }

    void ResumeGame() {
        board.SetIsGameStopped(false);
    }

    void StopGame() {
        board.SetIsGameStopped(true);
    }

    void OnReachGoalPoints() {
        round += 1;
        timer.ResetTimer();
    }
}