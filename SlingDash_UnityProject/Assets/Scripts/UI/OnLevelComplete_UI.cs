using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnLevelComplete_UI : MonoBehaviour 
{
	[SerializeField] Text coinsForCompletingLevel;
	[SerializeField] GameObject missionsParent;
	[SerializeField] List<Image> stars;
	[SerializeField] Text playerTotalCoins;

	void Start () 
	{
		LevelBased.LevelManager.Instance.OnLevelComplete_Event += OnLevelComplete;
		gameObject.SetActive(false);
	}
	
	void OnLevelComplete(LevelData levelData)
	{
		int totalCoinsEarned = 0;

		int onLevelCompleteCoins = LevelBased.LevelManager.Instance.coinsForCompletingLevel;

		coinsForCompletingLevel.text = "Coins for completing level: " + onLevelCompleteCoins.ToString("0");
		totalCoinsEarned += onLevelCompleteCoins;

		// Add missions on levelData to mission parent

		for (int i = 0; i < levelData.idsOfStarsPicked.Count; i++)
			stars[i].color = Color.white;

		for (int i = 0; i < LevelBased.LevelManager.Instance.currentNumberOfCollectedStars; i++)
			totalCoinsEarned += LevelBased.GameManager.Instance.coinsForCollectingStar;

		PersistentGameData.Instance.AddToCoins(totalCoinsEarned);

		playerTotalCoins.text = PersistentGameData.Instance.gameData.coins.ToString("0");

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
