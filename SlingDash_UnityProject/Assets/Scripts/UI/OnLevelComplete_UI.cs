using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnLevelComplete_UI : MonoBehaviour 
{
	[Header("Stats variables")]
	[SerializeField] Text coinsForCompletingLevel;
	[SerializeField] GameObject missionsParent;
	[SerializeField] List<Mission_UI> missionsUI;
	[SerializeField] List<Image> stars;
	[SerializeField] Text playerTotalCoins;
	[SerializeField] Text playerTotalNumberOfStars;

	[Header("Next level button")]
	[SerializeField] Button nextLevelButton;
	[SerializeField] GameObject unlockNextLevelInfo;
	[SerializeField] Text starsToUnlockNextLevelText;
	[SerializeField] GameObject nextLevelText;

	void Start () 
	{
		LevelBased.LevelManager.Instance.OnLevelComplete_Event += OnLevelComplete;
		gameObject.SetActive(false);
	}
	
	void OnLevelComplete(LevelData levelData)
	{
		int totalCoinsEarned = 0;

		int onLevelCompleteCoins = LevelBased.LevelManager.Instance.coinsForCompletingLevel;

		coinsForCompletingLevel.text = "Coins for completing level: " + onLevelCompleteCoins.ToString("0") + " x";
		totalCoinsEarned += onLevelCompleteCoins;

		foreach (Mission_UI mission in missionsUI)
		{
			mission.CrossMissionIfCompleted();
		}

		for (int i = 0; i < levelData.idsOfStarsPicked.Count; i++)
			stars[i].color = Color.white;

		for (int i = 0; i < LevelBased.LevelManager.Instance.currentNumberOfCollectedStars; i++)
			totalCoinsEarned += LevelBased.GameManager.Instance.coinsForCollectingStar;

		PersistentGameData.Instance.AddToCoins(totalCoinsEarned);

		playerTotalCoins.text = PersistentGameData.Instance.gameData.coins.ToString("0") + " x";
		playerTotalNumberOfStars.text = PersistentGameData.Instance.gameData.stars.ToString("0") + " x";

		string nextLevelID = LevelBased.LevelManager.Instance.nextLevelID;
		int starsToUnlockNextLevel = PersistentGameData.Instance.gameData.levelsData[nextLevelID].starsRequiredToUnlock;
		Debug.Log("Stars required for unlocking next level: " + starsToUnlockNextLevel);
		if (PersistentGameData.Instance.gameData.stars < starsToUnlockNextLevel)
		{
			nextLevelText.SetActive(false);
			unlockNextLevelInfo.SetActive(true);
			starsToUnlockNextLevelText.text = starsToUnlockNextLevel.ToString("0");
			nextLevelButton.interactable = false;
		}

		gameObject.SetActive(true);
	}

	void OnDestroy() 
	{
		LevelBased.LevelManager.Instance.OnLevelComplete_Event -= OnLevelComplete;
	}

	public void OnNextLevelPressed()
	{
		SceneManager.LoadScene(LevelBased.LevelManager.Instance.nextLevelID);
	}

	public void OnBackToMenuPressed()
	{
		SceneManager.LoadScene(0);
	}

	public void OnReplayPressed()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
