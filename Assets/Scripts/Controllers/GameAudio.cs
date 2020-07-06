using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour {
    private static GameAudio _instance;

    private void Awake() {
        if (_instance != null) {
            Destroy(this.gameObject);
        } else {
            LoadMyInstance();
        }
    }

    private void LoadMyInstance() {
        _instance = this;
        DontDestroyOnLoad(_instance);
    }
}