using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Achievements
{
	EARN_MORE_THAN_100_COINS,
	EARN_MORE_THAN_500_COINS,
	EARN_MORE_THAN_1000_COINS,
	COLLECT_5_STARS,
	COLLECT_10_STARS,
	COLLECT_30_STARS
}

public class AchievementsManager : Initializable
{
	[SerializeField] AchievementsScreen_UI achievementsScreen;

	public static AchievementsManager Instance { get; private set; }

	public Dictionary<Achievements, bool> achievementsData { get; private set;}

	private PersistentGameData persistentGameData;
	private Animator animator;

	private void Start() 
	{
		AchievementsManager[] achievementsManager = FindObjectsOfType<AchievementsManager>();
		if (achievementsManager[0] != this)
		{
			Destroy(gameObject);
		}

		animator = GetComponent<Animator>();
	}

	public override void Initialize()
	{
		Instance = this;

		persistentGameData = PersistentGameData.Instance;
		achievementsData = persistentGameData.gameData.achievementsData;

		if (achievementsData.Count <= 0)
		{
			achievementsData = new Dictionary<Achievements, bool>();
			achievementsData.Add(Achievements.EARN_MORE_THAN_100_COINS, false);
			achievementsData.Add(Achievements.EARN_MORE_THAN_500_COINS, false);
			achievementsData.Add(Achievements.EARN_MORE_THAN_1000_COINS, false);
			achievementsData.Add(Achievements.COLLECT_5_STARS, false);
			achievementsData.Add(Achievements.COLLECT_10_STARS, false);
			achievementsData.Add(Achievements.COLLECT_30_STARS, false);

			PersistentGameData.Instance.UpdateAchivements(achievementsData);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void TryToUnlockAchievement_Coins(int coins)
	{
		if (coins < 100) return;

		Achievements achievementToCheck = Achievements.EARN_MORE_THAN_100_COINS;

		if (coins >= 500) achievementToCheck = Achievements.EARN_MORE_THAN_500_COINS;
		if (coins >= 1000) achievementToCheck = Achievements.EARN_MORE_THAN_1000_COINS;

		// Achievement is already unlocked
		if (achievementsData[achievementToCheck] == true)
		{
			Debug.Log("Achievement already unlocked: " + achievementToCheck.ToString());
			return;
		}

		switch(achievementToCheck)
		{
			case Achievements.EARN_MORE_THAN_100_COINS:
				if (coins >= 100)
				{
					achievementsData[achievementToCheck] = true;
				} 
				break;

			case Achievements.EARN_MORE_THAN_500_COINS:
				if (coins >= 500)
				{
					achievementsData[achievementToCheck] = true;
				}
				break;

			case Achievements.EARN_MORE_THAN_1000_COINS:
				if (coins >= 1000)
				{
					achievementsData[achievementToCheck] = true;
				}
				break;
		}
		DisplayNotificationAndSaveData();
		Debug.Log("Achievement Unlocked: " + achievementToCheck.ToString());
	}

	public void TryToUnlockAchievement_Stars(int starsCollected)
	{
		if (starsCollected < 5) return;

		Achievements achievementToCheck = Achievements.COLLECT_5_STARS;

		if (starsCollected >= 10) achievementToCheck = Achievements.COLLECT_10_STARS;
		if (starsCollected >= 30) achievementToCheck = Achievements.COLLECT_30_STARS;

		// Achievement is already unlocked
		if (achievementsData[achievementToCheck] == true)
		{
			Debug.Log("Achievement already unlocked: " + achievementToCheck.ToString());
			return;
		}

		switch (achievementToCheck)
		{
			case Achievements.COLLECT_5_STARS:
				if (starsCollected >= 5)
				{
					achievementsData[achievementToCheck] = true;
				}
				break;

			case Achievements.COLLECT_10_STARS:
				if (starsCollected >= 10)
				{
					achievementsData[achievementToCheck] = true;
				}
				break;

			case Achievements.COLLECT_30_STARS:
				if (starsCollected >= 30)
				{
					achievementsData[achievementToCheck] = true;
				}
				break;
		}
		DisplayNotificationAndSaveData();
		Debug.Log("Achievement Unlocked: " + achievementToCheck.ToString());
	}

	private void DisplayNotificationAndSaveData()
	{
		animator.SetTrigger("OnUnlock");
		persistentGameData.UpdateAchivements(achievementsData);
	}
}
