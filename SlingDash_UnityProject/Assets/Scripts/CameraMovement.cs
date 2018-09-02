using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Player player;
	public Transform playerPos;
	public float yAxisOffset = 5f;
	public float lerpSpeed = 0.5f;

	private Vector3 cameraPos;

	void FixedUpdate ()
	{
		FollowPlayer();
	}

	void FollowPlayer()
	{
		if (!player.OnBulletTime())
		{
			cameraPos = new Vector3(0, playerPos.position.y + yAxisOffset, -10);
			Vector3 lerpMovement = Vector3.Lerp(transform.position, cameraPos, lerpSpeed);

			transform.position = lerpMovement;
		}
	}
}
