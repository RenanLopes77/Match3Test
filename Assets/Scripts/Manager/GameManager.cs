using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Board board = null;
    [SerializeField] private InGameMenu inGameMenu = null;
    [SerializeField] private Points points = null;
    [SerializeField] private Scene scene = null;
    [SerializeField] private Timer timer = null;
    public int round = 1;

    private void Start() {
        this.points.AddOnReachGoalPointsCallBack(this.OnReachGoalPoints);
        this.timer.AddCountdownFinishCallBack(this.StopGame);
        Time.timeScale = 1;
    }

    public void Restart() {
        this.scene.Restart();
    }

    void ResumeGame() {
        this.board.SetIsGameStopped(false);
    }

    public void PauseGame() {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        this.inGameMenu.OnPause(Time.timeScale == 0);
    }

    void StopGame() {
        this.board.SetIsGameStopped(true);
        this.inGameMenu.OnLose(this.points.GetPoints());
    }

    void OnReachGoalPoints() {
        this.round += 1;
        this.timer.ResetTimer();
    }
}