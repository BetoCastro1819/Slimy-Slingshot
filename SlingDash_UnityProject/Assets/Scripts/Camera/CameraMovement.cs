using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public PlayerSlimy player;
	public Transform playerPos;
	public float yAxisOffset = 5f;
	public float zAxisOffset = -10f;
	public float defaultLerpSpeed = 0.5f;
	public float bossFightLerpSpeed = 0.5f;
	public float constantSpeed = 2f;

	private Vector3 cameraGuide;
	private float lerpSpeed;

	private void Start()
	{
		cameraGuide = Vector3.zero;
		cameraGuide.y -= yAxisOffset;
		cameraGuide.z = transform.position.z;
		lerpSpeed = defaultLerpSpeed;
	}

	private void Update()
	{
		if (GameManager.GetInstance().GetState() == GameManager.GameState.PLAYING)
		{
			if (player != null) 
			{
				FollowPlayer();
			}
		}
	}

	void FollowPlayer()
	{
		/*
		if (LevelManager.GetInstance().LevelManagerState == LevelManager.LevelState.ON_BOSS_FIGHT)
		{
			lerpSpeed = bossFightLerpSpeed;
		}
		else
		{
			lerpSpeed = defaultLerpSpeed;
		}
		*/
		cameraGuide.y += constantSpeed * Time.unscaledDeltaTime;

		if (player.transform.position.y > cameraGuide.y)
		{
			cameraGuide.y = player.transform.position.y;
		}

		Vector3 cameraPos = new Vector3(0, cameraGuide.y + yAxisOffset, zAxisOffset);
        Vector3 lerpMovement = Vector3.Lerp(transform.position, cameraPos, lerpSpeed * Time.deltaTime);

	    transform.position = lerpMovement;
	}
}
