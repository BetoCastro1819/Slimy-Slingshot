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
			PlayerSlimy.OnLevelComplete_Event += OnLevelComplete;

			state = GameState.OnPlay;
		}

		void OnLevelComplete()
		{
			Debug.Log("On level complete");
			// Display level complete UI

			// Set the game to be on pause state
		}

		// Monobehaviour method
		void OnDestroy() 
		{
			PlayerSlimy.OnLevelComplete_Event -= OnLevelComplete;
		}
	}
}
