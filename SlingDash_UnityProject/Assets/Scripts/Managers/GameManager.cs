using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelCompleteScreen;
    public GameObject startText;
    public GameObject gameOverScreen;
    public Player player;

    // Meter Events
    public MeterDetector meterDetector;
    public List<MeterEvent> meterEventList;
    private int eventListIndex;
    public bool BossIsActive { get; set; }

    public float timeForGameOver = 2f;

	private Camera cam;
	private float timer = 0;

	private GameState gameState;
	public enum GameState
	{
		ON_START,
		PLAYING,
		PAUSE,
		GAME_OVER,
        LEVEL_COMPLETE
	}

    public void SetState(GameState gs)
	{
        gameState = gs;
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
		if (startText != null)
			startText.SetActive(true);

        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(false);

        eventListIndex = 0;
        BossIsActive = false;
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
                CheckForMeterEvents();
				break;
			case GameState.GAME_OVER:
				timer += Time.unscaledDeltaTime;
				if (timer > timeForGameOver) GameOver();
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
			if (startText != null)
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
		}
	}

    void CheckForMeterEvents()
    {
        // Check if there are any events on the list
        if (meterEventList.Count > 0 &&
            eventListIndex < meterEventList.Count)
        {
            if (meterDetector.GetMeters() >= meterEventList[eventListIndex].eventAt)
            {
                switch (meterEventList[eventListIndex].type)
                {
                    case EventType.SPAWN:
                        BossIsActive = true;
                        Instantiate(meterEventList[eventListIndex].prefabToSPAWN, Camera.main.transform);
                        break;
                    default:
                        break;
                }
                eventListIndex++;
            }
        }
    }

    public void LevelComplete()
    {
        gameState = GameState.LEVEL_COMPLETE;
        Time.timeScale = 0;
        levelCompleteScreen.SetActive(true);
    }

    void GameOver()
	{
        gameOverScreen.SetActive(true);
    }

	public void TutorialComplete()
	{
		TutorialManager.Get().SetTutorialPlayed(true);
	}
}
