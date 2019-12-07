using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour 
{
	[SerializeField] GameObject mainMenuOptions;
	[SerializeField] Text collectedStars;
	[SerializeField] Text numberOfCoins;

	private Animator animator;
	private GameObject currentObjectOnDisplay;

	private void Awake() 
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		UpdateStarsAndCoinsUI();
	}

	public void UpdateStarsAndCoinsUI()
	{
		collectedStars.text = "x " + PersistentGameData.Instance.gameData.stars.ToString("0");
		numberOfCoins.text = "x " + PersistentGameData.Instance.gameData.coins.ToString("0");
	}

	public void OnMenuExitFinished()
	{
		mainMenuOptions.SetActive(true);
		currentObjectOnDisplay = mainMenuOptions;
	}

	public void EnableObjectAndHideCurrent(GameObject objectToEnable)
	{
		currentObjectOnDisplay.SetActive(false);

		objectToEnable.SetActive(true);
		currentObjectOnDisplay = objectToEnable;
	}

	public void ScreenTapped()
	{
		animator.SetTrigger("ScreenTapped");
	}

	public void Play(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void Back()
	{
		MenuGameObject menuGameObject = currentObjectOnDisplay.GetComponent<MenuGameObject>();
		menuGameObject.previousScreen.SetActive(true);
		
		currentObjectOnDisplay.SetActive(false);

		currentObjectOnDisplay = menuGameObject.previousScreen;
	}
}
