using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMenu : MonoBehaviour {
    [SerializeField] private TMP_Text header = null;
    [SerializeField] private List<GameObject> onPauseGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> onLoseGameObjects = new List<GameObject>();

    public void OnPause(bool isPaused) {
        if (isPaused) {
            header.SetText("Paused");
        }
        SetGameObjectsActive(isPaused, onPauseGameObjects);
    }

    public void OnLose() {
        header.SetText("You lose");
        SetGameObjectsActive(true, onLoseGameObjects);
    }

    private void SetGameObjectsActive(bool isActive, List<GameObject> gos) {
        gos.ForEach(go => go.SetActive(isActive));
    }
}