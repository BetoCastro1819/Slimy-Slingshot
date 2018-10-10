using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

    bool isPaused = false;

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

    public void ActivateGameObject(GameObject obj) {
            obj.SetActive(true);
    }

    public void DeactivateGameObject(GameObject obj)
    {
            obj.SetActive(false);
    }

    public void Pause() {

        if (!isPaused)
        {
            if (GameManager.GetInstance() != null)
            {
                GameManager gameManager = GameManager.GetInstance();
                gameManager.SetState(GameManager.GameState.PAUSE);
            }

            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            if (GameManager.GetInstance() != null)
            {
                GameManager gameManager = GameManager.GetInstance();
                gameManager.SetState(GameManager.GameState.PLAYING);
            }

            isPaused = false;
            Time.timeScale = 1;
        }
    }
}
