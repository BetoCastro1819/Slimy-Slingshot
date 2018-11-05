using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpState : PlayerState
{
	public GameObject powerUpUI;
	public Text powerUpTextUI;
	public string powerUpName;

	public override void Enter()
	{
		base.Enter();

		powerUpUI.SetActive(true);
		powerUpTextUI.text = powerUpName;
	}

	public override void HandleInput()
	{
		base.HandleInput();

	}

	public override void UpdateState()
	{
		base.UpdateState();

	}

	public override void Exit()
	{
		base.Exit();

		powerUpUI.SetActive(false);
	}
}
