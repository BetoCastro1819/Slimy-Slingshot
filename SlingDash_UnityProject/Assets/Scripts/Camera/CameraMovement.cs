using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Player player;
	public Transform playerPos;
	public float yAxisOffset = 5f;
	public float zAxisOffset = -10f;
	public float lerpSpeed = 0.5f;
	public float constantSpeed = 2f;

	private Vector3 cameraGuide;

	private void Start()
	{
		cameraGuide = Vector3.zero;
		cameraGuide.y -= yAxisOffset;
		cameraGuide.z = transform.position.z;
	}

	private void LateUpdate()
	{
		if (GameManager.GetInstance().GetState() == GameManager.GameState.PLAYING)
		{
			if (player != null &&
				player.playerState == Player.PlayerState.ON_DASH) 
			{
				FollowPlayer();
			}
		}
	}

	void FixedUpdate ()
	{
		if (GameManager.GetInstance().GetState() == GameManager.GameState.PLAYING)
		{
			if (player != null &&
				player.playerState != Player.PlayerState.ON_DASH)
			{
				FollowPlayer();
			}
		}
	}

	void FollowPlayer()
	{
		cameraGuide.y += constantSpeed * Time.unscaledDeltaTime;

		if (player.transform.position.y > cameraGuide.y)
		{
			cameraGuide.y = player.transform.position.y;
		}

		Vector3 cameraPos = new Vector3(0, cameraGuide.y + yAxisOffset, zAxisOffset);
        Vector3 lerpMovement = Vector3.Lerp(transform.position, cameraPos, lerpSpeed * Time.fixedDeltaTime);

		if (player.playerState != Player.PlayerState.ON_DASH)
		{
		    transform.position = lerpMovement;
		}
		else
			transform.position = cameraGuide;

	}
}
