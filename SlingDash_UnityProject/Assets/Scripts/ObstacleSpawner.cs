using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	public GameObject obstaclePrefab;
	public Transform obstacelSpawnerPos;
	public Transform ObstaclesParent;

	private GameObject obstacle;

	public void SpawnObstacles()
	{
		obstacle = Instantiate(obstaclePrefab, obstacelSpawnerPos.position, Quaternion.identity);
		obstacle.transform.parent = ObstaclesParent;
	}
}
