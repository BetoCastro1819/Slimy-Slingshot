using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour 
{
	[SerializeField] AudioClip tapOnSplashScreenSound;
	[SerializeField] AudioClip buttonSound;
	[SerializeField] GameObject mainMenuOptions;
	[SerializeField] Text collectedStars;
	[SerializeField] Text numberOfCoins;

	private AudioManager audioManager;
	private Animator animator;
	private GameObject currentObjectOnDisplay;

	private void Awake() 
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		audioManager = AudioManager.Instance;
		audioManager.StopMusic();
		audioManager.RiseVolumeToGameplayLevel();
		
		UpdateStarsAndCoinsUI();
	}

	public void UpdateStarsAndCoinsUI()
	{
		collectedStars.text = "x " + PersistentGameData.Instance.gameData.stars.ToString();
		numberOfCoins.text = "x " + PersistentGameData.Instance.gameData.coins.ToString();
	}

	public void OnMenuExitFinished()
	{
		mainMenuOptions.SetActive(true);
		currentObjectOnDisplay = mainMenuOptions;
		audioManager.UpdateMusicState();
	}

	public void EnableObjectAndHideCurrent(GameObject objectToEnable)
	{
		currentObjectOnDisplay.SetActive(false);

		objectToEnable.SetActive(true);
		currentObjectOnDisplay = objectToEnable;

		PlayButtonSound();
	}

	public void ScreenTapped()
	{
		animator.SetTrigger("ScreenTapped");
		audioManager.PlayAudioClip(tapOnSplashScreenSound);
	}

	public void Play()
	{
		SceneManager.LoadScene(PersistentGameData.Instance.gameData.lastLevelPlayed);
		PlayButtonSound();
	}

	public void Back()
	{
		MenuGameObject menuGameObject = currentObjectOnDisplay.GetComponent<MenuGameObject>();
		menuGameObject.previousScreen.SetActive(true);
		
		currentObjectOnDisplay.SetActive(false);

		currentObjectOnDisplay = menuGameObject.previousScreen;

		PlayButtonSound();
	}

	private void PlayButtonSound()
	{
		audioManager.PlayAudioClip(buttonSound);
	}
}
