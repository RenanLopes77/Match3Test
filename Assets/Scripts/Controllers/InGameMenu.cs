using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMenu : MonoBehaviour {
    [SerializeField] private List<GameObject> onLoseGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> onPauseGameObjects = new List<GameObject>();
    [SerializeField] private Scene scene = null;
    [SerializeField] private TMP_Text header = null;

    public void OnPause(bool isPaused) {
        if (isPaused) {
            header.SetText("Paused");
        }
        SetGameObjectsActive(isPaused, onPauseGameObjects);
    }

    public void OnLose(int points) {
        header.SetText($"You lose \n {points}");
        SetGameObjectsActive(true, onLoseGameObjects);
    }

    public void OnClickHome() {
        this.scene.LoadScene(SceneNames.Menu);
    }

    private void SetGameObjectsActive(bool isActive, List<GameObject> gos) {
        gos.ForEach(go => go.SetActive(isActive));
    }
}