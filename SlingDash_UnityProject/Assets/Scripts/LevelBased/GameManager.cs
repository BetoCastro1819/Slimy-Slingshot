using System.Collections.Generic;
using UnityEngine;

namespace LevelBased
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] int m_coinsForCollectingStar;
		public int coinsForCollectingStar { get { return m_coinsForCollectingStar; } }

		public Dictionary<string, int> starsForUnlockingLevels { get; private set; }

		[System.Serializable]
		public struct StarsForEachLevel
		{
			public string levelID;
			public int starsRequiredToUnlock;
		}
		public List<StarsForEachLevel> starsForEachLevels;

		public static GameManager Instance { get; private set; }
		
		private void Awake()
		{
			GameManager[] gameManager = FindObjectsOfType<GameManager>();
			if (gameManager.Length == 1)
			{
				Initialize();
			}
		}

		private void Initialize()
		{
			Instance = this;

			Application.targetFrameRate = 60;

			starsForUnlockingLevels = new Dictionary<string, int>();
			for (int i = 0; i < starsForEachLevels.Count; i++)
			{
				starsForUnlockingLevels.Add(
					starsForEachLevels[i].levelID, starsForEachLevels[i].starsRequiredToUnlock
				);
			}
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
