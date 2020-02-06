using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour 
{
	[SerializeField] AudioClip buttonSound;
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
		AudioManager.Instance.StopMusic();
		AudioManager.Instance.RiseVolumeToGameplayLevel();
		
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
		PlayButtonSound();
		AudioManager.Instance.UpdateMusicState();
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
		AudioManager.Instance.PlayAudioClip(buttonSound);
	}
}
