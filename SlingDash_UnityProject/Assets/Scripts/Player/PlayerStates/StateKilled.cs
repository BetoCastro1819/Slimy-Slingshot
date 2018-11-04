using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKilled : PlayerState
{
	public GameObject playerDeathEffect;
	public GameObject playerOffScreenDeathEffect;

	public override void Enter()
	{
		base.Enter();

		Camera cam = Camera.main;

		// Cameras lower edge 
		float offBound = cam.transform.position.y - cam.orthographicSize - player.offBoundOffset;

		// If player is below camera's view
		if (transform.position.y < offBound)
		{
			// Instantiate respective death effect
			Vector3 effectPos = new Vector3(player.transform.position.x, cam.transform.position.y - cam.orthographicSize, 0);
			Instantiate(playerOffScreenDeathEffect, effectPos, Quaternion.identity);
		}
		else
		{
			Instantiate(playerDeathEffect, player.transform.position, Quaternion.identity);
		}

		player.gameObject.SetActive(false);

		//Debug.Log("StateMoving.Enter()");
	}

	public override void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			//Debug.Log("StateMoving -> StateAiming");

			player.StateAiming.Enter();
			player.SetState(player.StateAiming);
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

		player.health = 1;

		//Debug.Log("StateMoving.Exit()");
	}

}
