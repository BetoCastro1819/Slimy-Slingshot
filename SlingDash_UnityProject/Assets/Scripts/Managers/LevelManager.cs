using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class LevelData
{
	public string levelID;
	public int starsRequiredToUnlock;
	public List<int> idsOfStarsPicked;
	public Dictionary<string, bool> missions;

	public LevelData()
	{
		levelID = "";
		starsRequiredToUnlock = 0;
		idsOfStarsPicked = new List<int>();
		missions = new Dictionary<string, bool>();
	}
}

namespace LevelBased
{
	public class LevelManager : MonoBehaviour
	{
		#region Singleton
		public static LevelManager Instance { get; private set; }
		private void Awake() 
		{
			Instance = this;	
		}
		#endregion

		[SerializeField] string m_nextLevelID;
		[SerializeField] int m_coinsForCompletingLevel;
		[SerializeField] List<Star> stars;
		[SerializeField] List<Mission> missions;

		public event Action<LevelData> OnLevelComplete_Event;

		public string nextLevelID { get { return m_nextLevelID; } }
		public int coinsForCompletingLevel { get { return m_coinsForCompletingLevel; } }

		public enum GameState
		{
			OnPlay,
			OnPause
		}
		public GameState state { get; private set; }
		public LevelData levelData { get; private set; }

		public int currentNumberOfCollectedStars { get; private set; }

		void Start()
		{
			InitializeLevelID();
			InitializeMissions();
			SetupObserverMethods();
			InitializeStars();
			DestroyAlreadyPickedUpStars();

			PersistentGameData.Instance.SetLastLevelPlayed(levelData.levelID);

			state = GameState.OnPlay;
		}

		void InitializeLevelID()
		{
			string levelID = SceneManager.GetActiveScene().name;
			levelData = PersistentGameData.Instance.gameData.levelsData[levelID];
			levelData.levelID = levelID;
			//Debug.Log("Current levelID: " + levelData.levelID);
		}

		void InitializeMissions()
		{
			for (int i = 0; i < missions.Count; i++)
			{
				var persistentMissionsData = PersistentGameData.Instance.gameData.levelsData[levelData.levelID].missions;
				if (!persistentMissionsData.ContainsKey(missions[i].missionID))
					levelData.missions.Add(missions[i].missionID, missions[i].isComplete);
			}
		}

		void SetupObserverMethods()
		{
			PlayerSlimy.OnLevelComplete_Event += OnLevelComplete;
			Player_UI.OnPause_Event += OnPause;
			PauseMenu_UI.OnResumeGame_Event += OnResume;
			Star.OnStarPickedUp_Event += OnStarPickedUp;
		}

		void InitializeStars()
		{
			currentNumberOfCollectedStars = 0;

			for (int starID = 0; starID < stars.Count; starID++)
				stars[starID].starID = starID;
		}

		void DestroyAlreadyPickedUpStars()
		{
			for (int i = 0; i < stars.Count; i++)
			{
				if (levelData.idsOfStarsPicked.Contains(stars[i].starID))
					Destroy(stars[i].gameObject);
			}
		}

		void OnLevelComplete()
		{
			//Debug.Log("On level complete");

			PersistentGameData.Instance.AddToStarsCollected(currentNumberOfCollectedStars);
			UpdatePersistentMissionsStates();
			PersistentGameData.Instance.UpdateLevelData(levelData);

			OnLevelComplete_Event(levelData);

			OnPause();
		}

		void UpdatePersistentMissionsStates()
		{
			for (int i = 0; i < missions.Count; i++)
			{
				if (missions[i].IsComplete())
				{
					levelData.missions[missions[i].missionID] = missions[i].isComplete;
				}

				if (levelData.missions[missions[i].missionID])
					Debug.LogFormat("Mission {0} was completed", missions[i].missionID);
			}
		}

		void OnResume()
		{
			Time.timeScale = 1;
		}

		void OnPause()
		{
			Time.timeScale = 0;
		}

		void OnStarPickedUp(int starID)
		{
			levelData.idsOfStarsPicked.Add(starID);
			currentNumberOfCollectedStars++;
		}

		// Monobehaviour method
		void OnDestroy() 
		{
			PlayerSlimy.OnLevelComplete_Event -= OnLevelComplete;
			Player_UI.OnPause_Event -= OnPause;
			PauseMenu_UI.OnResumeGame_Event -= OnResume;

			Star.OnStarPickedUp_Event -= OnStarPickedUp;
		}

	}
}
