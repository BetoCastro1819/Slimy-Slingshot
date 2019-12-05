using UnityEngine;
using UnityEngine.SceneManagement;

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

		[SerializeField] string m_nextLevel;
		public string nextLevel { get { return m_nextLevel; } }

		public int currentScore { get; private set; }
		public int highscore { get; private set; }

		string levelID;

		public enum GameState
		{
			OnPlay,
			OnPause
		}
		public GameState state { get; private set; }

		void Start()
		{
			levelID = SceneManager.GetActiveScene().name;

			state = GameState.OnPlay;
			
			PlayerSlimy.OnLevelComplete_Event += OnLevelComplete;
			Player_UI.OnPause_Event += OnPause;
			PauseMenu_UI.OnResumeGame_Event += OnResume;
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

		// Monobehaviour method
		void OnDestroy() 
		{
			PlayerSlimy.OnLevelComplete_Event -= OnLevelComplete;
			Player_UI.OnPause_Event -= OnPause;
			PauseMenu_UI.OnResumeGame_Event -= OnResume;
		}
	}
}
