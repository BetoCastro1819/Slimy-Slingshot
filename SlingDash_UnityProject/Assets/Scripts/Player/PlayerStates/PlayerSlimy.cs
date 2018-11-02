using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlimy : MonoBehaviour
{
	#region Singleton
	private static PlayerSlimy instance;
	public static PlayerSlimy Get()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}
	#endregion

	[Header("Player States")]
	public StateMoving stateMoving;
	public StateAiming stateAiming;

	[Header("Player Components")]
	public Rigidbody2D m_Rigidbody;

	private PlayerState m_State;

	void Start ()
	{
		m_State = stateMoving;
	}

	void Update ()
	{
		m_State.HandleInput();
		m_State.UpdateState();
	}

	public void SetState(PlayerState state)
	{
		m_State = state;
	}

}
