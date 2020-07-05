using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
  [SerializeField] private TMP_Text text;
  private int currentTime = 0;
  private int defaultTime = 120;

  private void Start() {
    this.currentTime = this.defaultTime;
    StartCoroutine(Countdown());
  }

  IEnumerator Countdown() {

    while (this.currentTime >= 0) {
      SetCurrentTimeText();
      yield return new WaitForSeconds(1f);
      this.currentTime -= 1;
    }

    yield return null;
  }

  private void SetCurrentTimeText() {
    this.text.SetText(currentTime.ToString());
  }
}