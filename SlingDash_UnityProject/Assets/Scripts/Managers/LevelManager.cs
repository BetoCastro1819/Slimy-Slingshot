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

	public GameObject tutorialEndTrigger;
	public bool onTutorial = true;

	public Transform spawnersParent;                        // GameObject parent of the group of spawners
	public Transform spawnerLeft;
	public Transform spawnerCenter;
	public Transform spawnerRight;

	public MeterDetector meterDetector;                     // Player's current max height reached in meters
	public List<MeterEvent> meterEventList;                 // List of the events based on player's current travelled distance (E.g: Boss Fight)
	public float distBtwnObjects = 10f;                     // Distance between each spawned object
	public float spawningOffset = 2f;                       // Offset to spawn object out of camera view

	public List<WeightedGameObject> listOfWeightedObjs;		// List of all possible objects to be spawned in the level

	private int sumOfWeights;                               // Sum of all the weights of the gameObjects 
	private float spawnObjectAt;                            // Position where to spawn next object

	public bool BossIsActive { get; set; }                  // Is true whenever a Boss Event is currently active
	public LevelState LevelManagerState { get; set; }

	private List<Transform> listOfSpawners;

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
		MOVING_ENEMY,
		SHOOTING_ENEMY,
		POWER_UP
	}

	public enum LevelState
	{
		ON_TUTORIAL,
		STANDARD_GAMEPLAY,
		ON_BOSS_FIGHT
		// NEXT_LEVEL?
	}

	void Start ()
	{
		listOfSpawners = new List<Transform>
		{
			spawnerLeft,
			spawnerCenter,
			spawnerRight
		};

		spawnObjectAt = spawnersParent.transform.position.y;
		LevelManagerState = LevelState.ON_TUTORIAL;
		BossIsActive = false;
		onTutorial = true;
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
			case LevelState.ON_BOSS_FIGHT:
				CheckBossCondition();
				break;
		}
	}

	void LevelTutorial()
	{
		if (!onTutorial)
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

        for (int i = 0; i < listOfWeightedObjs.Count; i++)
        {
            sumOfWeights += listOfWeightedObjs[i].weight;
        }

        int randomWeight = Random.Range(0, sumOfWeights);
        for (int i = 0; i < listOfWeightedObjs.Count; i++)
        {
            if (randomWeight < listOfWeightedObjs[i].weight)
            {
				if (listOfWeightedObjs[i].key == GameObjectKey.LEFT_BRANCH)
				{
					Instantiate(listOfWeightedObjs[i].go, spawnerLeft.transform.position, Quaternion.identity);
				}
				else if (listOfWeightedObjs[i].key == GameObjectKey.RIGHT_BRANCH)
				{
					Instantiate(listOfWeightedObjs[i].go, spawnerRight.transform.position, Quaternion.identity);
				}
				else
				{
					int spawnerIndex = Random.Range(0, listOfSpawners.Count);             
					Instantiate(listOfWeightedObjs[i].go, listOfSpawners[spawnerIndex].transform.position, Quaternion.identity);
				}
				return;
            }

            randomWeight -= listOfWeightedObjs[i].weight; 
        }
    }

	void CheckBossCondition()
	{
		if (!BossIsActive)
		{
			// Enable portal for next sector
			LevelManagerState = LevelState.STANDARD_GAMEPLAY;
		}
	}

	void CheckForMeterEvents()
	{
		for (int i = 0; i < meterEventList.Count; i++)
		{
			switch (meterEventList[i].type)
			{
				case EventType.SPAWN:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt && !BossIsActive)
					{
						// Spawn boss at next meter event point
						SpawnBoss(meterEventList[i].prefabToSPAWN);    
					}
					break;
					/*
				case EventType.ENABLE_OBSTACLES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (obstaclesSpawnerLeft.activeInHierarchy == false)
							obstaclesSpawnerLeft.SetActive(true);

						if (obstaclesSpawnerRight.activeInHierarchy == false)
							obstaclesSpawnerRight.SetActive(true);

					}
					break;
				case EventType.ENABLE_MOVING_ENEMIES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (movingEnemiesSpawner.activeInHierarchy == false)
							movingEnemiesSpawner.SetActive(true);
					}
					break;
				case EventType.ENABLE_SHOOTING_ENEMIES:
					if (meterDetector.GetMetersTravelled() >= meterEventList[i].eventAt)
					{
						if (shootingEnemiesSpawner.activeInHierarchy == false)
							shootingEnemiesSpawner.SetActive(true);
					}
					break;
				default:
					break;
					*/
			}
		}
	}

	private void SpawnBoss(GameObject spawn)
	{
		BossIsActive = true;
		LevelManagerState = LevelState.ON_BOSS_FIGHT;
		Instantiate(spawn, Camera.main.transform, false);
	}
}
