using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
	public Slider energyBar;
    public Text meterText;
    public Text scoreText;
    public Text coinsText;

	private CoinManager coinManager;
	private ScoreManager scoreManager;

	private int currentCoins;
	private int currentScore;

	#region Singleton
	private static UI_Manager instance;
	public static UI_Manager Get()
	{
		return instance;
	}
	private void Awake()
	{
		instance = this;
    }
	#endregion

	private void Start()
	{
		coinManager = CoinManager.Get();
		scoreManager = ScoreManager.Get();

		currentCoins = coinManager.GetCoins();
		currentScore = scoreManager.GetScore();

		if (coinsText != null)
		{
			coinsText.text = currentCoins.ToString("0000");
		}

		if (scoreText != null)
		{
			scoreText.text = currentScore.ToString("0");
		}
	}

	private void Update()
	{
		if (coinManager.GetCoins() > currentCoins)
		{
			currentCoins = coinManager.GetCoins();
			coinsText.text = currentCoins.ToString("0");
		}

		if (scoreManager.GetScore() > currentScore)
		{
			currentScore = scoreManager.GetScore();
			scoreText.text = currentScore.ToString("0");
		}
	}
}
