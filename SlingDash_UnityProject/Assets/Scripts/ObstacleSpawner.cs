using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	public GameObject obstaclePrefab;
	public List<Transform> obstacleSpawners;
	public Transform ObstaclesParent;

	private GameObject obstacle;

	public void SpawnObstacles()
	{
		int rand = Random.Range(0, obstacleSpawners.Count - 1);
		for (int i = 0; i < obstacleSpawners.Count; i++)
		{
			if (i != rand)
			{
				obstacle = Instantiate(obstaclePrefab, obstacleSpawners[i].position, Quaternion.identity);
				obstacle.transform.parent = ObstaclesParent;
			}
		}
	}
}
