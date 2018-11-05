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

	public bool BoosIsActive { get; set; }                  // Is true whenever a Boss Event is currently active

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

	void Start ()
	{
		listOfSpawners = new List<Transform>
		{
			spawnerLeft,
			spawnerCenter,
			spawnerRight
		};

		spawnObjectAt = spawnersParent.transform.position.y;
        instance.BoosIsActive = false;
	}
	
	void Update ()
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
}
