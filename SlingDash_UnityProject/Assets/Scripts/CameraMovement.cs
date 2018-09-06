using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Player player;
	public Transform playerPos;
	public float yAxisOffset = 5f;
	public float zAxisOffset = -10f;
	public float lerpSpeed = 0.5f;
	public float constantSpeed = 2f;

	private Vector3 cameraPos;
	private Vector3 cameraGuide;

	private void Start()
	{
		cameraGuide = Vector3.zero;
	}

	void FixedUpdate ()
	{
		if (!player.OnBulletTime())
			FollowPlayer();
	}

	void FollowPlayer()
	{
        /*
		cameraGuide.y += constantSpeed * Time.deltaTime;

		if (player.transform.position.y > cameraGuide.y)
			cameraGuide.y = player.transform.position.y;
        */

		cameraPos = new Vector3(0, player.transform.position.y + yAxisOffset, zAxisOffset);
		Vector3 lerpMovement = Vector3.Lerp(transform.position, cameraPos, lerpSpeed);

		transform.position = lerpMovement;
	}
}
