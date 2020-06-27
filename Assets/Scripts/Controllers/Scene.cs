using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {

    [Button]
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}