using System.Collections.Generic;
using UnityEngine;

public class AchievementsScreen_UI : MonoBehaviour 
{
	[SerializeField] Transform achievementsContainer;
	[SerializeField] GameObject achievementItemPrefab;

	public void SetupAchievementsDisplay(Dictionary<Achievements, bool> achievementsData) 
	{
		GameObject achievement = Instantiate(achievementItemPrefab);
		achievement.transform.parent = achievementsContainer;
		achievement.transform.localScale = Vector3.one;
	}
}
