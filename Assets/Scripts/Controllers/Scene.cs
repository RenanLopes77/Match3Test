using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames {
    Menu,
    Game
}

public class Scene : MonoBehaviour {

    [Button]
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(SceneNames sceneName) {
        SceneManager.LoadScene(sceneName.ToString());
    }
}