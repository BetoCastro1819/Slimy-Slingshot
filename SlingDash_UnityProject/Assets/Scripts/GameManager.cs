using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public ObstacleSpawner obstacleSpawner;
	public float offsetToSpawnObstacles = 2f;
	public float distBtwnObjs = 5f;

	private Camera cam;
	private float camTopEdgePos;
	private float posToSpawnObstacle;

	#region Singleton
	private static GameManager instance;
	public static GameManager GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion

	private void Start()
	{
		cam = Camera.main;
		camTopEdgePos = cam.transform.position.y + cam.orthographicSize;
		posToSpawnObstacle = camTopEdgePos + offsetToSpawnObstacles + distBtwnObjs;
	}

	void Update ()
	{
		camTopEdgePos = cam.transform.position.y + cam.orthographicSize;
		if (camTopEdgePos + offsetToSpawnObstacles > posToSpawnObstacle)
		{
			// spawn object
			obstacleSpawner.SpawnObstacles();
			posToSpawnObstacle = camTopEdgePos + offsetToSpawnObstacles + distBtwnObjs;
		}
	}
}
