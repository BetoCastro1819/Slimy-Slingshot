using UnityEngine;

namespace LevelBased
{
	public class GameManager : MonoBehaviour
	{
		#region Singleton
		public static GameManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;

			Application.targetFrameRate = 60;

			DontDestroyOnLoad(this.gameObject);
		}
		#endregion

		void Start()
		{
		}

		void Update()
		{
		}
	}
}
