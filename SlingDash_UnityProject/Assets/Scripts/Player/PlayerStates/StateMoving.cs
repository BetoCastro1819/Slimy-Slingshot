using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMoving : PlayerState
{
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

	public override void Enter()
	{
		base.Enter();

		Debug.Log("StateMoving is now active");
	}
}
