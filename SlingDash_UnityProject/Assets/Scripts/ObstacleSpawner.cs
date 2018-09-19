using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	public List<GameObject> obstacles;
	public List<Transform> obstacleSpawners;
	public Transform ObstaclesParent;
	public float offsetToSpawnObstacles = 2f;
	public float distBtwnObjs = 5f;

	private float posToSpawnObstacle;

	private void Start()
	{
		posToSpawnObstacle = transform.position.y + distBtwnObjs;
	}

	private void Update()
	{
		if (transform.position.y + offsetToSpawnObstacles > posToSpawnObstacle)
		{
			SpawnObstacles();
			posToSpawnObstacle = transform.position.y + distBtwnObjs;
		}
	}


	private void SpawnObstacles()
	{
		int randSpawner = Random.Range(0, obstacleSpawners.Count);
		int randObstacle = Random.Range(0, obstacles.Count);

		GameObject obstacle = Instantiate(obstacles[randObstacle], obstacleSpawners[randSpawner].position, obstacleSpawners[randSpawner].rotation);
		obstacle.transform.parent = ObstaclesParent;
	}
}
