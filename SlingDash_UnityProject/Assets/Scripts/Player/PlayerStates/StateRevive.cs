using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRevive : PlayerState
{
	public GameObject reviveParticles;
	public GameObject reviveScreenTrigger;

	private Vector3 revivePos;
	private float timeToRevivePlayer;
	private float timer;


	public override void Enter()
	{
		base.Enter();

		//player.gameObject.SetActive(true);

		ParticleSystem particles = reviveParticles.GetComponent<ParticleSystem>();
		Instantiate(reviveParticles, revivePos, Quaternion.identity);

		timeToRevivePlayer = particles.main.startLifetimeMultiplier;
		timer = 0;

		//Debug.Log("StateMoving.Enter()");
	}

	public override void HandleInput()
	{
		base.HandleInput();

	}

	public override void UpdateState()
	{
		base.UpdateState();

		timer += Time.deltaTime;
		if (timer > timeToRevivePlayer)
		{
			GameManager.GetInstance().EnablePlayer();
			Exit();
		}
	}

	public override void Exit()
	{
		base.Exit();

		player.StateMoving.Enter();
		player.SetState(player.StateMoving);

		//Debug.Log("StateMoving.Exit()");
	}

	public void SetRevivePosition(Vector3 playerPos)
	{
		revivePos = playerPos;
	}
}
