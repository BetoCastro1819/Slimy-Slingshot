using UnityEngine;
using UnityEngine.UI;

public class Mission_UI : MonoBehaviour 
{
	[SerializeField] Mission mission;
	[SerializeField] GameObject crossline;

	private void Start() 
	{
		Text missionDescription = GetComponent<Text>();
		missionDescription.text = "- " + mission.missionDescription;	
	}

	public void CrossMissionIfCompleted() 
	{
		string levelID = LevelBased.LevelManager.Instance.levelData.levelID;
		bool currentMissionState = PersistentGameData.Instance.gameData.levelsData[levelID].missions[mission.missionID];

		crossline.SetActive(currentMissionState);
	}
}
