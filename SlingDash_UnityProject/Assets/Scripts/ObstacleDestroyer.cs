using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
	void Update ()
	{
		if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
			Destroy(this.gameObject);
	}
}
