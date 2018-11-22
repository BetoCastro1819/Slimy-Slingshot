using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
	public float offsetToDestroy = 4f;

	void Update ()
	{
		if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - offsetToDestroy)
			Destroy(gameObject);
	}
}
