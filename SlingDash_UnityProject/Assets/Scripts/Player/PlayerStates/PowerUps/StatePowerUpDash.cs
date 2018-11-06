using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePowerUpDash : PowerUpState
{
	[Header("Sprites")]
	public Sprite defaultSprite;
	public Sprite onHoldSprite;

	[Header("Game Objects")]
	public GameObject clearScreenTrigger;
	public GameObject blurParticlesEffect;

	public CameraMovement cameraMovement;

	public int meterToDash = 50;
	public float dashVelocity = 10f;
	public float timeToDash = 1f;
	public float forceAddedAfterDash = 10f;
	public float dashCameraLerpSpeed = 10f;

	private float timer;
	private float yPosToReach;
	private float originalCameraLerpSpeed;

	private CameraShake cameraShake;

	public override void Enter()
	{
		base.Enter();

		player.GetComponent<SpriteRenderer>().sprite = onHoldSprite;

		cameraShake = Camera.main.GetComponent<CameraShake>();

		player.transform.up = Vector3.up;

		originalCameraLerpSpeed = cameraMovement.lerpSpeed;
		cameraMovement.lerpSpeed = dashCameraLerpSpeed;

		clearScreenTrigger.SetActive(true);

		player.GetComponent<CircleCollider2D>().enabled = false;

		Debug.Log("Dash is now active!");

		yPosToReach = player.transform.position.y + meterToDash;

		timer = 0;
	}

	public override void HandleInput()
	{
		base.HandleInput();

		timer += Time.deltaTime;
		if (timer > timeToDash)
		{
			Dash();
		}
		else
		{
			player.PlayerRigidbody.velocity = Vector2.zero;
		}

	}

	public override void UpdateState()
	{
		base.UpdateState();

	}

	public override void Exit()
	{
		base.Exit();

		blurParticlesEffect.SetActive(false);

		cameraMovement.lerpSpeed = originalCameraLerpSpeed;

		clearScreenTrigger.SetActive(false);

		player.PlayerRigidbody.velocity = Vector2.zero;
		player.PlayerRigidbody.AddForce(player.transform.up * forceAddedAfterDash);

		player.GetComponent<CircleCollider2D>().enabled = true;

		player.StateMoving.Enter();
		player.SetState(player.StateMoving);
	}

	private void Dash()
	{
		if (player.transform.position.y < yPosToReach)
		{
			if (blurParticlesEffect.activeInHierarchy == false)
			{
				blurParticlesEffect.SetActive(true);
			}
			player.GetComponent<SpriteRenderer>().sprite = defaultSprite;
			cameraShake.StartShake();
			player.transform.position += Vector3.up * dashVelocity * Time.deltaTime;
		}
		else
		{
			Exit();
		}
	}
}
