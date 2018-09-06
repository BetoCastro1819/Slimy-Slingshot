using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

    public void SceneLoad(string name) {
        SceneManager.LoadScene(name);
    }

    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RestartCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
