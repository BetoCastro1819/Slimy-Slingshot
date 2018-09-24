using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelCompleteScreen;
	public GameObject startText;
	public Player player;

	private Camera cam;

	private GameState gameState;
	public enum GameState
	{
		ON_START,
		PLAYING,
		PAUSE,
		GAME_OVER,
        LEVEL_COMPLETE
	}

	public GameState GetState()
	{
		return gameState;
	}

	#region Singleton
	private static GameManager instance;
	public static GameManager GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;

		// Caps processing frame rate at 60 FPS
		Application.targetFrameRate = 60;
	}
	#endregion

	private void Start()
	{
		cam = Camera.main;
		gameState = GameState.ON_START;
		Time.timeScale = 0;
		startText.SetActive(true);
        if (levelCompleteScreen != null)
        {
            levelCompleteScreen.SetActive(false);
        }
	}

	void Update ()
	{
		GameFSM();
	}

	void GameFSM()
	{
		switch (gameState)
		{
			case GameState.ON_START:
				StartGame();
				break;
			case GameState.PLAYING:
				CheckForPlayer();
				break;
			case GameState.GAME_OVER:
				break;
			case GameState.PAUSE:
				break;
            case GameState.LEVEL_COMPLETE:
                // enable LevelComplete screen
                LevelComplete();
                break;
        }
	}

	void StartGame()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			//Debug.Log("GAME STARTED");
			startText.SetActive(false);
			Time.timeScale = 1;
			gameState = GameState.PLAYING;
		}
	}

	void CheckForPlayer()
	{
		if (player != null)
		{
			float playerOffBound = cam.transform.position.y - cam.orthographicSize;

			//if (player.transform.position.y < playerOffBound)
				//Debug.Log("OFF BOUNDS");
		}
	}

    public void LevelComplete()
    {
        gameState = GameState.LEVEL_COMPLETE;
        Time.timeScale = 0;
        levelCompleteScreen.SetActive(true);

        // call ui manager
    }
}
