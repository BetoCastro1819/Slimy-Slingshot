using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
	public Transform playerPos;
	
	void Update ()
	{
		Vector3 wallPos = new Vector3(transform.position.x, playerPos.transform.position.y, 0);
		transform.position = wallPos;
	}
}
