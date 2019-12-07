using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_UI : MonoBehaviour 
{
	[SerializeField] Mission mission;
	[SerializeField] GameObject crossline;

	public void CrossMissionIfCompleted() 
	{
		string levelID = LevelBased.LevelManager.Instance.levelData.levelID;
		bool currentMissionState = PersistentGameData.Instance.gameData.levelsData[levelID].missions[mission.missionID];

		crossline.SetActive(currentMissionState);
	}
}
