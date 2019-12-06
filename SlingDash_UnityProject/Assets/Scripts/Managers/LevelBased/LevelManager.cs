using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class LevelData
{
	public string levelID;
	public int highscore;
	public int starsRequiredToUnlock;
	public List<int> idsOfStarsPicked;

	// public Mission[] missions

	public LevelData()
	{
		levelID = "";
		highscore = 0;
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
		[SerializeField] List<Star> stars;
		[SerializeField] GameObject starPrefab;

		public string nextLevelID { get { return m_nextLevelID; } }
		public int currentScore { get; private set; }
		public int highscore { get; private set; }

		public enum GameState
		{
			OnPlay,
			OnPause
		}
		public GameState state { get; private set; }

		LevelData levelData;

		void Start()
		{
			string levelID = SceneManager.GetActiveScene().name;
			levelData = PersistentGameData.Instance.gameData.levelsData[levelID];

			state = GameState.OnPlay;
			
			PlayerSlimy.OnLevelComplete_Event += OnLevelComplete;
			Player_UI.OnPause_Event += OnPause;
			PauseMenu_UI.OnResumeGame_Event += OnResume;

			Star.OnStarPickedUp_Event += OnStarPickedUp;

			DestroyAlreadyPickedUpStars();
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
			// Display level complete UI

			// Set the game to be on pause state
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
