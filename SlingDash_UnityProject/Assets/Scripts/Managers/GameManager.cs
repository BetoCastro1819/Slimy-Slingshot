using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject levelCompleteScreen;
    public Player player;

	// UI elements
    public GameObject startText;
	public GameObject gameOverScreen;
	public GameObject reviveScreen;
	public int coinsForRevive = 50;

	// Meter Events
	public MeterDetector meterDetector;
    public List<MeterEvent> meterEventList;

	// Boss
	private int spawnBossAt;
    public bool BossIsActive { get; set; }

	// Obstacles
	public GameObject obstaclesSpawnerLeft;
	public GameObject obstaclesSpawnerRight;

	// Moving enemies
	public GameObject movingEnemiesSpawner;

	// Shooting enemies
	public GameObject shootingEnemiesSpawner;

    public float timeForGameOver = 2f;

	private CoinManager coinManager;
	private float timer = 0;
    private bool isGameOver = false;

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
		gameState = GameState.ON_START;
		Time.timeScale = 0;
		if (startText != null)
			startText.SetActive(true);

        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(false);

		if (reviveScreen != null)
		{
			reviveScreen.SetActive(false);
		}

		coinManager = CoinManager.Get();


		/* WILL CHANGE THIS AFTER PROTOTYPE MODE */
		//spawnBossAt = meterEventList[3].eventAt;	// 3 = BossEvent 
		/*
        BossIsActive = false;
		obstaclesSpawnerLeft.SetActive(false);
		obstaclesSpawnerRight.SetActive(false);
		movingEnemiesSpawner.SetActive(false);
		shootingEnemiesSpawner.SetActive(false);
		*/
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
                isGameOver = false;
				break;
			case GameState.GAME_OVER:
				if (!isGameOver)
				{
					timer += Time.unscaledDeltaTime;
					if (timer > timeForGameOver)
					{
						timer = 0;
						GameOver();
					}
				}
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
		Time.timeScale = 0;

		if (startText != null)
			startText.SetActive(true);


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
			//float playerOffBound = cam.transform.position.y - cam.orthographicSize;
		}
	}

    void CheckForMeterEvents()
    {
		for (int i = 0; i < meterEventList.Count; i++)
		{
			switch (meterEventList[i].type)
			{
				case EventType.SPAWN:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt && !BossIsActive)
					{
                          SpawnBoss(meterEventList[i].prefabToSPAWN);     // Spawn boss at next meter event point
					}
					break;
				case EventType.ENABLE_OBSTACLES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (obstaclesSpawnerLeft.activeInHierarchy == false)
							obstaclesSpawnerLeft.SetActive(true);

						if (obstaclesSpawnerRight.activeInHierarchy == false)
							obstaclesSpawnerRight.SetActive(true);

					}
					break;
				case EventType.ENABLE_MOVING_ENEMIES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (movingEnemiesSpawner.activeInHierarchy == false)
							movingEnemiesSpawner.SetActive(true);
					}
					break;
				case EventType.ENABLE_SHOOTING_ENEMIES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (shootingEnemiesSpawner.activeInHierarchy == false)
							shootingEnemiesSpawner.SetActive(true);
					}
					break;
				default:
					break;
			}
		}
	}

	private void SpawnBoss(GameObject spawn)
	{
		BossIsActive = true;
        Instantiate(spawn, Camera.main.transform,false);
	}

    public void LevelComplete()
    {
        gameState = GameState.LEVEL_COMPLETE;
        Time.timeScale = 0;
        levelCompleteScreen.SetActive(true);
    }

	void GameOver()
	{
		isGameOver = true;

		if (reviveScreen != null)
		{
			reviveScreen.SetActive(true);
		}
		else
		{
			gameOverScreen.SetActive(true);
		}
	}
    public bool IsGameOver()
	{
        return isGameOver;
    }
}
