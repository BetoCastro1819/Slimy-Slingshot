using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAiming : PlayerState
{
	public override void HandleInput()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			Debug.Log("StateAiming -> StateMoving");

			player.stateMoving.Enter();
			player.SetState(player.stateMoving);
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

		Debug.Log("StateAiming is now active");
	}
}
