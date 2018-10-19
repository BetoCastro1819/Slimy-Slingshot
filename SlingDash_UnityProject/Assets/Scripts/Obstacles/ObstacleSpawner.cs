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

    private GameManager gm;
	private float posToSpawnObstacle;

	private void Start()
	{
        gm = GameManager.GetInstance();
		posToSpawnObstacle = transform.position.y + distBtwnObjs;
	}

	private void Update()
	{
        if (!gm.BossIsActive)
        {
            if (transform.position.y + offsetToSpawnObstacles > posToSpawnObstacle)
            {
                SpawnObstacles();
                posToSpawnObstacle = transform.position.y + distBtwnObjs;
            }
        }
    }


	private void SpawnObstacles()
	{
		int obstacleIndex = 0;
		if (obstacles.Count > 1)
			obstacleIndex = Random.Range(0, obstacles.Count);

		int randSpawner = Random.Range(0, obstacleSpawners.Count);

		GameObject obstacle = Instantiate(obstacles[obstacleIndex], obstacleSpawners[randSpawner].position, Quaternion.identity);
		obstacle.transform.parent = ObstaclesParent;
	}
}
