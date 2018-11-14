using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	#region Singleton
	private static LevelManager instance;
	public static LevelManager GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion

	public int sectionIndex = 1;

	[Header("Spawners")]
	public Transform spawnersParent;                        // GameObject parent of the group of spawners

	public Transform spawnerLeft;
	public Transform spawnerCenter;
	public Transform spawnerRight;

	[Header("Meter Events")]
	public float timeToSpawnBoss;
	public GameObject UI_BossIncoming;
	public MeterDetector meterDetector;                     // Player's current max height reached in meters
	public List<MeterEvent> meterEventList;                 // List of the events based on player's current travelled distance (E.g: Boss Fight)

	[Header("List of GameObjects per section")]
	public List<List<WeightedGameObject>> listOfSections;
	public List<WeightedGameObject> objectsSection1;		// List of all possible objects to be spawned in the level
	public List<WeightedGameObject> objectsSection2;		// List of all possible objects to be spawned in the level

	public float distBtwnObjects = 10f;                     // Distance between each spawned object
	public float spawningOffset = 2f;                       // Offset to spawn object out of camera view
	
	public LevelState LevelManagerState { get; set; }
	public bool BossIsActive { get; set; }                  // Is true whenever a Boss Event is currently active
	public bool OnTutorial { get; set; }
	public int CurrentSectionIndex { get; set; }

	private List<Transform> listOfSpawners;
	private int sumOfWeights;                               // Sum of all the weights of the gameObjects 
	private float spawnObjectAt;                            // Position where to spawn next object

	private float spawnBossTimer;
	private GameObject bossToSpawn;

	[System.Serializable]
	public struct WeightedGameObject
	{
		public GameObject go;
		public int weight;
		public GameObjectKey key;
	}

	public enum GameObjectKey
	{
		LEFT_BRANCH,
		RIGHT_BRANCH,
		NOT_BRANCH,
	}

	public enum LevelState
	{
		ON_TUTORIAL,
		STANDARD_GAMEPLAY,
		BOSS_INCOMING,
		ON_BOSS_FIGHT,
		PLAYER_KILLED_BOSS
	}

	void Start ()
	{
		listOfSections = new List<List<WeightedGameObject>>
		{
			objectsSection1,
			objectsSection2
		};

		listOfSpawners = new List<Transform>
		{
			spawnerLeft,
			spawnerCenter,
			spawnerRight
		};

		LevelManagerState = LevelState.ON_TUTORIAL;

		spawnObjectAt = spawnersParent.transform.position.y;
		OnTutorial = true;
		CurrentSectionIndex = sectionIndex;

		UI_BossIncoming.SetActive(false);
		BossIsActive = false;
		spawnBossTimer = 0;

		//Debug.Log("List of Sections: " + listOfSections.Count);
	}

	void Update ()
	{
		UpdateState();
	}

	void UpdateState()
	{
		switch (LevelManagerState)
		{
			case LevelState.ON_TUTORIAL:
				LevelTutorial();
				break;

			case LevelState.STANDARD_GAMEPLAY:
				CheckPositionToSpawnObjects();
				CheckForMeterEvents();
				break;

			case LevelState.BOSS_INCOMING:
				OnBossFightEnter();
				break;

			case LevelState.ON_BOSS_FIGHT:
				CheckBossCondition();
				break;

			case LevelState.PLAYER_KILLED_BOSS:
				GoToNextSection();
				break;
		}
	}

	void LevelTutorial()
	{
		if (!OnTutorial)
		{
			LevelManagerState = LevelState.STANDARD_GAMEPLAY;
		}
	}

	void CheckPositionToSpawnObjects()
	{
		if (spawnersParent.transform.position.y + spawningOffset > spawnObjectAt)
		{
			SpawnObject();
			spawnObjectAt = spawnersParent.transform.position.y + distBtwnObjects;
		}
	}

	void SpawnObject()
	{
        sumOfWeights = 0;

		var currentSectionObjs = listOfSections[CurrentSectionIndex];

		for (int i = 0; i < currentSectionObjs.Count; i++)
		{
			sumOfWeights += currentSectionObjs[i].weight;
		}

        int randomWeight = Random.Range(0, sumOfWeights);
        for (int i = 0; i < currentSectionObjs.Count; i++)
        {
            if (randomWeight < currentSectionObjs[i].weight)
            {
				if (currentSectionObjs[i].key == GameObjectKey.LEFT_BRANCH)
				{
					Instantiate(currentSectionObjs[i].go, spawnerLeft.transform.position, Quaternion.identity);
				}
				else if (currentSectionObjs[i].key == GameObjectKey.RIGHT_BRANCH)
				{
					Instantiate(currentSectionObjs[i].go, spawnerRight.transform.position, Quaternion.identity);
				}
				else
				{
					int spawnerIndex = Random.Range(0, listOfSpawners.Count);             
					Instantiate(currentSectionObjs[i].go, listOfSpawners[spawnerIndex].transform.position, Quaternion.identity);
				}
				return;
            }

            randomWeight -= currentSectionObjs[i].weight; 
        }
    }



	void CheckForMeterEvents()
	{
		for (int i = 0; i < meterEventList.Count; i++)
		{
			switch (meterEventList[i].type)
			{
				case EventType.SPAWN:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt * (CurrentSectionIndex + 1) && !BossIsActive)
					{
						LevelManagerState = LevelState.BOSS_INCOMING;
						SetBossToSpawn(meterEventList[i].prefabToSpawn[CurrentSectionIndex]);
					}
					break;
			}
		}
	}

	void OnBossFightEnter()
	{
		if (UI_BossIncoming.activeInHierarchy == false)
		{
			UI_BossIncoming.SetActive(true);
		}

		spawnBossTimer += Time.unscaledDeltaTime;
		if (spawnBossTimer >= timeToSpawnBoss)
		{
			// Spawn boss at next meter event point
			SpawnBoss(bossToSpawn);
		}
	}

	void CheckBossCondition()
	{
		if (!BossIsActive)
		{
			// Enable portal for next sector
			LevelManagerState = LevelState.PLAYER_KILLED_BOSS;
		}
	}

	void SetBossToSpawn(GameObject boss)
	{
		bossToSpawn = boss;
	}

	private void SpawnBoss(GameObject spawn)
	{
		Instantiate(spawn, Camera.main.transform, false);
		spawnBossTimer = 0;
		UI_BossIncoming.SetActive(false);
		BossIsActive = true;
		LevelManagerState = LevelState.ON_BOSS_FIGHT;
	}

	private void GoToNextSection()
	{
		if (CurrentSectionIndex + 1 < listOfSections.Count)
		{
			CurrentSectionIndex++;
		}

		LevelManagerState = LevelState.STANDARD_GAMEPLAY;
	}
}
