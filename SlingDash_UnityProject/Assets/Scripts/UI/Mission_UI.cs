using UnityEngine;
using UnityEngine.UI;

public class Mission_UI : MonoBehaviour 
{
	[SerializeField] Mission mission;
	[SerializeField] GameObject crossline;
	[SerializeField] float crosslineMaxScaleX;
	[SerializeField] float crossAnimationSpeed;

	private bool shouldAnimate;

	private void Start() 
	{
		Text missionDescription = GetComponent<Text>();
		missionDescription.text = "- " + mission.missionDescription;	
		shouldAnimate = false;

		crossline.transform.localScale = new Vector2(0, crossline.transform.localScale.y);
	}

	public void CrossMissionIfCompleted() 
	{
		string levelID = LevelBased.LevelManager.Instance.levelData.levelID;
		bool currentMissionState = PersistentGameData.Instance.gameData.levelsData[levelID].missions[mission.missionID];

		crossline.SetActive(currentMissionState);

		shouldAnimate = currentMissionState;
	}

	private void Update()
	{
		if (!shouldAnimate) return;

		crossline.transform.localScale = new Vector2(
			crossline.transform.localScale.x + crossAnimationSpeed,
			crossline.transform.localScale.y
		);

		if (crossline.transform.localScale.x >= crosslineMaxScaleX)
			shouldAnimate = false;
	}
}
