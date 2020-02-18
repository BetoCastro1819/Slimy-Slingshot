using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelButton_UI : MonoBehaviour 
{
	[SerializeField] string levelSceneName;
	[SerializeField] GameObject levelNumber;
	[SerializeField] GameObject infoToUnlockLevel;
	[SerializeField] Text starsToUnlock;

	private int starsRequiredToUnlock;

	void Start()
	{
		starsRequiredToUnlock = LevelBased.GameManager.Instance.starsForUnlockingLevels[levelSceneName];
		//PersistentGameData.Instance.UpdateRequiredStarsToUnlockLevel(levelSceneName, starsRequiredToUnlock);

		//Debug.LogFormat("Stars required for unlockig {0}: {1}", levelSceneName, starsRequiredToUnlock);

		Button button = GetComponent<Button>();
		button.onClick.AddListener(OpenLevel); 

		if (PersistentGameData.Instance.gameData.stars < starsRequiredToUnlock)
		{
			button.interactable = false;

			levelNumber.SetActive(false);

			infoToUnlockLevel.SetActive(true);
			starsToUnlock.text = starsRequiredToUnlock.ToString("0");
		}
	}
	
	void OpenLevel() 
	{
		SceneManager.LoadScene(levelSceneName);
	}
}
