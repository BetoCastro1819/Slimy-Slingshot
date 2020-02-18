using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public string lastLevelPlayed;
	public int timesPlayed;
	public int stars;
	public int coins;
	public bool musicIsEnabled;
	public bool sfxAreEnabled;
	public string currentPlayerTrail;
	public List<string> trailsPurchased;
	public Dictionary<string, LevelData> levelsData;
	public Dictionary<Achievements, bool> achievementsData;

	public GameData(string firstLevelSceneName) 
	{
		lastLevelPlayed = firstLevelSceneName;
		timesPlayed = 0;
		stars = 0;
		coins = 0;
		musicIsEnabled = true;
		sfxAreEnabled = true;
		currentPlayerTrail = "trails/standard";
		trailsPurchased = new List<string>();
		levelsData = new Dictionary<string, LevelData>();
		achievementsData = new Dictionary<Achievements, bool>();
	}
}

public class PersistentGameData : Initializable
{
	[SerializeField] string firstLevelSceneName;
	[SerializeField] MainMenu_UI mainMenu;
	[SerializeField] string gameDataFileName;

	public static PersistentGameData Instance { get; private set; }
	public GameData gameData { get; private set; }

	private void Start()
	{
		PersistentGameData[] persistentGameData = FindObjectsOfType<PersistentGameData>();
		if (persistentGameData[0] != this)
		{
			Destroy(gameObject);
		}
	}

	public override void Initialize()
	{
		Instance = this;

		LoadLocalGameData();

		AddOneToTimesPlayed();

		Debug.LogFormat("Current amount of collected stars: {0}", gameData.stars);

		DontDestroyOnLoad(this.gameObject);
	}

	private void LoadLocalGameData()
	{
		string gameDataLocalPath = gameDataFileName;
		if (!Application.isEditor)
			gameDataLocalPath = Application.persistentDataPath + "/" + gameDataFileName;

		if (File.Exists(gameDataLocalPath))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream stream = new FileStream(gameDataLocalPath, FileMode.Open);

			gameData = binaryFormatter.Deserialize(stream) as GameData;
			stream.Close();
		}
		else
		{
			gameData = new GameData(firstLevelSceneName);

			for (int i = 0; i < LevelBased.GameManager.Instance.starsForUnlockingLevels.Count; i++)
			{
				string currentLevelID = LevelBased.GameManager.Instance.starsForEachLevels[i].levelID;
				gameData.levelsData.Add(currentLevelID, new LevelData());
			}

			UpdateLocalGameData();

			Debug.Log("There is no local saved data");
		}
	}

	public void UpdateSfxToggle(bool sfxAreEnabled)
	{
		gameData.sfxAreEnabled = sfxAreEnabled;
		UpdateLocalGameData();
	}

	public void UpdateMusicToggle(bool muscicIsEnabled)
	{
		gameData.musicIsEnabled = muscicIsEnabled;
		UpdateLocalGameData();
	}

	public void SetLastLevelPlayed(string levelSceneName)
	{
		gameData.lastLevelPlayed = levelSceneName;
		UpdateLocalGameData();
	}

	public void UpdateRequiredStarsToUnlockLevel(string levelID, int starsToUnlock)
	{
		gameData.levelsData[levelID].starsRequiredToUnlock = starsToUnlock;
		UpdateLocalGameData();
	}

	public void AddTrailToPurchaseList(string trailPath)
	{
		gameData.trailsPurchased.Add(trailPath);
		UpdateLocalGameData();
	}

	public void SetTrailAsCurrent(string trailPath)
	{
		gameData.currentPlayerTrail = trailPath;
		UpdateLocalGameData();
	}

	public void SubstractFromCoins(int amountToSubstract)
	{
		gameData.coins -= amountToSubstract;
		if (gameData.coins < 0) gameData.coins = 0;

		TryToUpdateMainMenuValues();

		UpdateLocalGameData();
	}

	public void AddToCoins(int coinsToAdd)
	{
		gameData.coins += coinsToAdd;

		TryToUpdateMainMenuValues();

		AchievementsManager.Instance.TryToUnlockAchievement_Coins(gameData.coins);

		UpdateLocalGameData();
	}

	public void AddToStarsCollected(int starsToAdd)
	{
		gameData.stars += starsToAdd;

		TryToUpdateMainMenuValues();

		AchievementsManager.Instance.TryToUnlockAchievement_Stars(gameData.stars);

		UpdateLocalGameData();
	}

	public void UpdateLevelData(LevelData levelData)
	{
		Debug.Log("Updating level data of: " + levelData.levelID);
		LevelData currentLevel = gameData.levelsData[levelData.levelID];

		currentLevel.starsRequiredToUnlock = levelData.starsRequiredToUnlock;

		if (currentLevel.idsOfStarsPicked.Count < levelData.idsOfStarsPicked.Count)
			currentLevel.idsOfStarsPicked = levelData.idsOfStarsPicked;

		gameData.levelsData[levelData.levelID] = currentLevel; 
		UpdateLocalGameData();
	}

	public void UpdateAchivements(Dictionary<Achievements, bool> achievementsData)
	{
		gameData.achievementsData = achievementsData;
		UpdateLocalGameData();
	}

	private void TryToUpdateMainMenuValues()
	{
		if (!mainMenu)
		{
			mainMenu = FindObjectOfType<MainMenu_UI>();

			if (mainMenu)
				mainMenu.UpdateStarsAndCoinsUI();
		}
		else
		{
			mainMenu.UpdateStarsAndCoinsUI();
		}
	}

	private void AddOneToTimesPlayed()
	{
		gameData.timesPlayed++;
		UpdateLocalGameData();
	}

	private void UpdateLocalGameData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();

		string gameDataLocalPath = gameDataFileName;
		if (!Application.isEditor)
			gameDataLocalPath = Application.persistentDataPath + "/" + gameDataFileName;

		FileStream stream = new FileStream(gameDataLocalPath, FileMode.Create);

		binaryFormatter.Serialize(stream, gameData);
		stream.Close();
	}
}
