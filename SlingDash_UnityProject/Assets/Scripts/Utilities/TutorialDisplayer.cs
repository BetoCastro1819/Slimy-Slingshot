using UnityEngine;

public class TutorialDisplayer : MonoBehaviour 
{
	[SerializeField] GameObject tutorialCanvas;

	void Start()
	{
		if (PersistentGameData.Instance.gameData.timesPlayed <= 1)
			tutorialCanvas.SetActive(true);
		else
			tutorialCanvas.SetActive(false);
	}
}
