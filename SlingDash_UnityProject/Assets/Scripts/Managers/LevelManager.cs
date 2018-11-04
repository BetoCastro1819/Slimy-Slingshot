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
    public int weightForBranchesLeft;
    public int weightForBranchesRight;
    public int weightForMovingEnemies;
    public int weightForShootingEnemies;

	public List<GameObject> objectList;                     // List of posible objects to spawn on the MIDDLE spawners
	public MeterDetector meterDetector;					    // Player's current max height reached in meters
	public List<MeterEvent> meterEventList;				    // List of the events based on player's current travelled distance (E.g: Boss Fight)
	public float distBtwnObjects = 10f;                     // Distance between each spawned object
	public float spawningOffset = 2f;					    // Offset to spawn object out of camera view

	public bool BoosIsActive { get; set; }                  // Is true whenever a Boss Event is currently active

    private int sumOfWeights;                               // Sum of all the weights of the gameObjects 
    private float spawnObjectAt;						    // Position where to spawn next object

    private List<WeightedGameObject> listOfWeightedObjs;
	private List<Transform> listOfSpawners;

	//[System.Serializable]
    private struct WeightedGameObject
	{
        public GameObject go;
        public int weight;
    }

	void Start ()
	{
		listOfSpawners = new List<Transform>
		{
			spawnerLeft,
			spawnerCenter,
			spawnerRight
		};

		listOfWeightedObjs = new List<WeightedGameObject>();

		spawnObjectAt = spawnersParent.transform.position.y;
        instance.BoosIsActive = false;

        for (int i = 0; i < objectList.Count; i++)
        {
			WeightedGameObject weightedGameObject = new WeightedGameObject
			{
				go = objectList[i]
			};

			switch (objectList[i].tag)
            {
                case "BranchLeft":
                    weightedGameObject.weight = weightForBranchesLeft;
                    break;
				case "BranchRight":
					weightedGameObject.weight = weightForBranchesRight;
					break;
				case "MovingEnemy":
                    weightedGameObject.weight = weightForMovingEnemies;
                    break;
                case "ShootingEnemy":
                    weightedGameObject.weight = weightForShootingEnemies;
                    break;
            }

            listOfWeightedObjs.Add(weightedGameObject);
        }
        Debug.Log(listOfWeightedObjs.Count);
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
				if (listOfWeightedObjs[i].go.tag == "BranchLeft")
				{
					Instantiate(listOfWeightedObjs[i].go, spawnerLeft.transform.position, Quaternion.identity);
				}
				else if (listOfWeightedObjs[i].go.tag == "BranchRight")
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
