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

	// public Mission[] missions

	public LevelData()
	{
		levelID = "";
		starsRequiredToUnlock = 0;
		idsOfStarsPicked = new List<int>();
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
			string levelID = SceneManager.GetActiveScene().name;
			levelData = PersistentGameData.Instance.gameData.levelsData[levelID];
			levelData.levelID = levelID;
			Debug.Log("Current levelID: " + levelData.levelID);

			state = GameState.OnPlay;
			
			PlayerSlimy.OnLevelComplete_Event += OnLevelComplete;
			Player_UI.OnPause_Event += OnPause;
			PauseMenu_UI.OnResumeGame_Event += OnResume;

			Star.OnStarPickedUp_Event += OnStarPickedUp;

			InitializeStars();
			DestroyAlreadyPickedUpStars();
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
			Debug.Log("On level complete");

			PersistentGameData.Instance.AddToStarsCollected(currentNumberOfCollectedStars);
			PersistentGameData.Instance.UpdateLevelData(levelData);

			OnLevelComplete_Event(levelData);

			OnPause();
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
