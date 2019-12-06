using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelButton_UI : MonoBehaviour 
{
	[SerializeField] string levelSceneName;

	//private LevelData levelData;
	private int starsRequiredToUnlock;

	void Start()
	{
		starsRequiredToUnlock = LevelBased.GameManager.Instance.starsForUnlockingLevels[levelSceneName];

		Debug.LogFormat("Stars required for unlockig {0}: {1}", levelSceneName, starsRequiredToUnlock);

		Button button = GetComponent<Button>();
		button.onClick.AddListener(OpenLevel); 

		if (PersistentGameData.Instance.gameData.stars < starsRequiredToUnlock)
		{
			button.interactable = false;
		}
	}
	
	void OpenLevel() 
	{
		SceneManager.LoadScene(levelSceneName);
	}
}
