using UnityEngine;
using UnityEngine.UI;

public class DebugScreen : MonoBehaviour
{
	[SerializeField] Button buttonToEnableDebugScreen;
	[SerializeField] GameObject debugButtons;
	[SerializeField] int coinAmountToAdd;
	[SerializeField] int starsToAdd;

	private void Start()
	{
		debugButtons.SetActive(false);
		buttonToEnableDebugScreen.onClick.AddListener(EnableDebugScreen);
	}

	private void EnableDebugScreen()
	{
		debugButtons.SetActive(true);
	}

	public void HideDebugScreen()
	{
		debugButtons.SetActive(false);
	}

	public void AddStars()
	{
		PersistentGameData.Instance.AddToStarsCollected(starsToAdd);
	}

	public void AddCoins()
	{
		PersistentGameData.Instance.AddToCoins(coinAmountToAdd);
	}
}
