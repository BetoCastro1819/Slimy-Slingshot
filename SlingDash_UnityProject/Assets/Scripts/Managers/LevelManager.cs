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

	public Transform spawnersParent;					// GameObject parent of the group of spawners
	public List<Transform> spawners;                    // Will hold the posible position to spawn the objects
	public List<GameObject> leftSideObjs;				// List of posible objects to spawn on the LEFT SIDE spawners
	public List<GameObject> rigthSideObjs;              // List of posible objects to spawn on the RIGHT SIDE spawners
	public List<GameObject> middleObjs;                 // List of posible objects to spawn on the MIDDLE spawners
	public MeterDetector meterDetector;					// Player's current max height reached in meters
	public List<MeterEvent> meterEventList;				// List of the events based on player's current travelled distance (E.g: Boss Fight)
	public float distBtwnObjects = 10f;                 // Distance between each spawned object
	public float spawningOffset = 2f;					// Offset to spawn object out of camera view

	public bool BoosIsActive { get; set; }              // Is true whenever a Boss Event is currently active

	private float spawnObjectAt;						// Position where to spawn next object

	void Start ()
	{
		instance.BoosIsActive = false;
		spawnObjectAt = spawnersParent.transform.position.y;
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
	}

	void SelectObjectAndSpawn(List<GameObject> objectToSpawn, int spawnerIndex)
	{
		int	objectIndex = Random.Range(0, objectToSpawn.Count);
		Instantiate(objectToSpawn[objectIndex], spawners[spawnerIndex].transform.position, Quaternion.identity);
	}

}
