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

	public Transform spawnersParent;					    // GameObject parent of the group of spawners
	public List<Transform> spawners;                        // Will hold the posible position to spawn the objects
    public int weightForBranches;
    public int weightForMovingEnemies;
    public int weightForShootingEnemies;

	public List<GameObject> objectList;                     // List of posible objects to spawn on the MIDDLE spawners
	//public List<GameObject> leftSideObjs;				    // List of posible objects to spawn on the LEFT SIDE spawners
	//public List<GameObject> rigthSideObjs;                // List of posible objects to spawn on the RIGHT SIDE spawners
	public MeterDetector meterDetector;					    // Player's current max height reached in meters
	public List<MeterEvent> meterEventList;				    // List of the events based on player's current travelled distance (E.g: Boss Fight)
	public float distBtwnObjects = 10f;                     // Distance between each spawned object
	public float spawningOffset = 2f;					    // Offset to spawn object out of camera view

	public bool BoosIsActive { get; set; }                  // Is true whenever a Boss Event is currently active

    private int sumOfWeights;                               // Sum of all the weights of the gameObjects 
    private float spawnObjectAt;						    // Position where to spawn next object

    private List<WeightedGameObject> listOfWeightedObjs;
    public struct WeightedGameObject
    {
        public GameObject go;
        public int weight;
    }

	void Start ()
	{
        listOfWeightedObjs = new List<WeightedGameObject>();

        spawnObjectAt = spawnersParent.transform.position.y;
        instance.BoosIsActive = false;


        for (int i = 0; i < objectList.Count; i++)
        {
            WeightedGameObject weightedGameObject = new WeightedGameObject();

            weightedGameObject.go = objectList[i];

            switch (objectList[i].tag)
            {
                case "Branch":
                    weightedGameObject.weight = weightForBranches;
                    break;
                case "MovingEnemy":
                    weightedGameObject.weight = weightForBranches;
                    break;
                case "ShootingEnemy":
                    weightedGameObject.weight = weightForBranches;
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
		int spawnerIndex = Random.Range(0, spawners.Count);				// Select random spanwer from the list of spawners
        sumOfWeights = 0;

        for (int i = 0; i < listOfWeightedObjs.Count; i++)
        {
            sumOfWeights += listOfWeightedObjs[i].weight;
        }

        int randomWeight = Random.Range(0, sumOfWeights);

        for (int i = 0; i < listOfWeightedObjs.Count; i++)
        {
            if (randomWeight < sumOfWeights)
            {
                Instantiate(listOfWeightedObjs[i].go, spawners[spawnerIndex].transform.position, Quaternion.identity);
                return;
            }

            randomWeight -= listOfWeightedObjs[i].weight; 
        }

        /*
		if (spawners[spawnerIndex].transform.position.x < 0)            // LEFT SIDE SPAWNER
		{
			SelectObjectAndSpawn(leftSideObjs, spawnerIndex);
		}
		else if (spawners[spawnerIndex].transform.position.x > 0)       // RIGHT SIDE SPAWNER
		{
			SelectObjectAndSpawn(rigthSideObjs, spawnerIndex);
		}
		else															// MIDDLE SPAWNER
		{
			SelectObjectAndSpawn(middleObjs, spawnerIndex);
		}
        */
    }

    void SelectObjectAndSpawn(List<GameObject> objectToSpawn, int spawnerIndex)
	{
		int	objectIndex = Random.Range(0, objectToSpawn.Count);
		Instantiate(objectToSpawn[objectIndex], spawners[spawnerIndex].transform.position, Quaternion.identity);
	}

    /*
    int sum_of_weight = 0;
    for(int i=0; i<num_choices; i++) 
    {
       sum_of_weight += choice_weight[i];
    }

    int rnd = random(sum_of_weight);
    for(int i=0; i<num_choices; i++) 
    {
      if(rnd < choice_weight[i])
        return i;

      rnd -= choice_weight[i];
    }
     */
}
