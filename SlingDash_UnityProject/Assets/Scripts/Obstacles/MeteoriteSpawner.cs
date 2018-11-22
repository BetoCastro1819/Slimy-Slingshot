using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
	public GameObject meteoritePrefab;
	public GameObject alert;
	public float spawnRateInSeconds = 10f;
	public float alertPlayerDuration = 2f;

	private PlayerSlimy player;
	private float spawnTimer;

	private SpawnerState state;
	private enum SpawnerState
	{
		FOLLOW_PLAYER,
		SPAWN_METEORITE
	}

	void Start ()
	{
		player = GameManager.GetInstance().player;
		state = SpawnerState.FOLLOW_PLAYER;

		spawnTimer = 0;
	}
	
	void Update ()
	{
		UpdateSpawnerState();
	}

	private void UpdateSpawnerState()
	{
		switch (state)
		{
			case SpawnerState.FOLLOW_PLAYER:
				FollowPlayerOnX();
				CheckForTimeToSpawn();
				break;

			case SpawnerState.SPAWN_METEORITE:
				SpawnMeteorite();
				break;
		}
	}

	private void FollowPlayerOnX()
	{
		Vector2 followPos = new Vector2(player.transform.position.x, transform.position.y);

		transform.position = followPos;
	}

	private void CheckForTimeToSpawn()
	{
		spawnTimer += Time.unscaledDeltaTime;

		// Alert the player before spawning meteorite
		if (spawnTimer >= spawnRateInSeconds - alertPlayerDuration)
		{
			// Enable alert
			alert.SetActive(true);
		}

		if (spawnTimer >= spawnRateInSeconds)
		{
			// Disable alert
			alert.SetActive(false);

			spawnTimer = 0;
			state = SpawnerState.SPAWN_METEORITE;
		}
	}

	private void SpawnMeteorite()
	{
		Instantiate(meteoritePrefab, transform.position, transform.rotation);

		state = SpawnerState.FOLLOW_PLAYER;
	}
}
