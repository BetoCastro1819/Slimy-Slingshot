﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMoving : PlayerState
{
	public Sprite playerDefaultSprite;

	public override void Enter()
	{
		base.Enter();

		player.SetSprite(playerDefaultSprite);

		Debug.Log("StateMoving.Enter()");
	}

	public override void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Debug.Log("StateMoving -> StateAiming");

			player.stateAiming.Enter();
			player.SetState(player.stateAiming);
		}

		base.HandleInput();
	}

	public override void UpdateState()
	{
		base.UpdateState();



	}

	public override void Exit()
	{
		base.Exit();

		Debug.Log("StateMoving.Exit()");
	}
}
