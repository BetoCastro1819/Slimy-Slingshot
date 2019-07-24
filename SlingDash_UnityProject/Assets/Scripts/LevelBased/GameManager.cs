using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBased
{
	public class GameManager : MonoBehaviour
	{
		private PersistentGameData gameData;

		#region Singleton
		private static GameManager instance;
		public static GameManager GetInstance()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;

			Application.targetFrameRate = 60;

			gameData = new PersistentGameData();
			gameData.AddOneToTimesPlayed();

			DontDestroyOnLoad(this.gameObject);
		}
		#endregion

		void Start()
		{

		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.D))
			{
				Debug.Log("PlayerPrefs cleared");
				PlayerPrefs.DeleteAll();
			}
		}
	}
}
