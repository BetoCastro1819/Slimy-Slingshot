using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnLevelComplete_UI : MonoBehaviour 
{
	[Header("Audio")]
	[SerializeField] AudioClip buttonSound;

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

	[Header("Animation variables")]
	[SerializeField] float timeIntervalBetweenElements;
	[SerializeField] float waitTimeToIncreaseNumberValue;

	private LevelData levelData;

	void Start () 
	{
		LevelBased.LevelManager.Instance.OnLevelComplete_Event += OnLevelComplete;
		gameObject.SetActive(false);
	}
	
	void OnLevelComplete(LevelData levelData)
	{
		gameObject.SetActive(true);
		this.levelData = levelData;

		UpdateUIElements();
	}

	void UpdateUIElements()
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
		{
			stars[i].color = Color.white;
		}

		for (int i = 0; i < LevelBased.LevelManager.Instance.currentNumberOfCollectedStars; i++)
			totalCoinsEarned += LevelBased.GameManager.Instance.coinsForCollectingStar;

		int currentPlayerCoins = PersistentGameData.Instance.gameData.coins;
		playerTotalCoins.text = currentPlayerCoins.ToString("0") + " x";
		PersistentGameData.Instance.AddToCoins(totalCoinsEarned);

		while (currentPlayerCoins < PersistentGameData.Instance.gameData.coins)
		{
			currentPlayerCoins++;
			playerTotalCoins.text = currentPlayerCoins.ToString("0") + " x";
		}

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
	} 

	void OnDestroy() 
	{
		LevelBased.LevelManager.Instance.OnLevelComplete_Event -= OnLevelComplete;
	}

	public void OnNextLevelPressed()
	{
		PlayButtonSound();
		SceneManager.LoadScene(LevelBased.LevelManager.Instance.nextLevelID);
	}

	public void OnBackToMenuPressed()
	{
		PlayButtonSound();
		SceneManager.LoadScene(0);
	}

	public void OnReplayPressed()
	{
		PlayButtonSound();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void PlayButtonSound()
	{
		AudioManager.Instance.PlayAudioClip(buttonSound);
	}
}
