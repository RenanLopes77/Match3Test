using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
  [SerializeField] private TMP_Text text = null;
  private int currentTime = 0;
  private int defaultTime = 120;
  private Coroutine countdownCoroutine = null;
  private List<Action> onCountdownFinishCallbacks = new List<Action>();

  private void Start() {
    this.currentTime = this.defaultTime;
    StartCountdown();
  }

  private void StartCountdown() {
    this.countdownCoroutine = StartCoroutine(Countdown());
  }

  private void StopCountdown() {
    StopCoroutine(this.countdownCoroutine);
  }

  IEnumerator Countdown() {
    while (this.currentTime >= 0) {
      SetCurrentTimeText();
      yield return new WaitForSeconds(1f);
      this.currentTime -= 1;
    }

    OnCountdownFinish();
  }

  void OnCountdownFinish() {
    this.onCountdownFinishCallbacks.ForEach(callBack => callBack());
  }

  private void SetCurrentTimeText() {
    this.text.SetText(currentTime.ToString());
  }

  public void ResetTimer() {
    this.currentTime = defaultTime;
    StopCountdown();
    StartCountdown();
  }

  public void AddCountdownFinishCallBack(Action action) {
    this.onCountdownFinishCallbacks.Add(action);
  }
}