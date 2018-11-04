using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
	protected PlayerSlimy player;

	public virtual void Start()
	{
		player = PlayerSlimy.Get();
		//Debug.Log("PlayerState.Start()");
	}

	public virtual void Enter()
	{ }

	public virtual void Exit()
	{ }

	public virtual void UpdateState ()
	{ }

	public virtual void HandleInput()
	{ }
}
