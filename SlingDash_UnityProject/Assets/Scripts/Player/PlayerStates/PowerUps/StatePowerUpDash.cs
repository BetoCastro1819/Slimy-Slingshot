using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePowerUpDash : PlayerState
{
	public int meterToDash = 50;
	public float dashVelocity = 10f;
	public float timeToDash = 1f;

	private float timer;

	public override void Enter()
	{
		base.Enter();

		player.GetComponent<CircleCollider2D>().enabled = false;
		player.PlayerRigidbody.velocity = Vector2.zero;

		timer = 0;
	}

	public override void HandleInput()
	{
		base.HandleInput();

		timer += Time.deltaTime;
		if (timer > timeToDash)
		{
			Dash();
			Exit();
		}

	}

	public override void UpdateState()
	{
		base.UpdateState();

	}

	public override void Exit()
	{
		base.Exit();

	}

	private void Dash()
	{

	}
}
