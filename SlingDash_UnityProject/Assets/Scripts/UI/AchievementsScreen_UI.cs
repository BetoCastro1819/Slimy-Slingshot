using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsScreen_UI : MonoBehaviour 
{
	[SerializeField] Transform achievementsContainer;
	[SerializeField] GameObject achievementItemPrefab;

	void Start()
	{
		Dictionary<Achievements, bool> achievementData = AchievementsManager.Instance.achievementsData;
		SetupAchievementsDisplay(achievementData);
	}

	void SetupAchievementsDisplay(Dictionary<Achievements, bool> achievementsData) 
	{
		for (int index = 0; index < achievementsData.Count; index++)
		{
			Achievements achievementEnumKey = (Achievements)index;
			string achievementString = achievementEnumKey.ToString().Replace("_", " ");
			bool achievementCompleted = achievementsData[achievementEnumKey];

			GameObject achievement = Instantiate(achievementItemPrefab);
			achievement.transform.parent = achievementsContainer;
			achievement.transform.localScale = Vector3.one;

			Text achievementText = achievement.GetComponentInChildren<Text>();
			achievementText.text = achievementString;
			
			if (achievementCompleted)
				achievementText.color = Color.green;

			//Debug.Log("Generating UI element for achievement: " + achievementString);
		}
	}
}
