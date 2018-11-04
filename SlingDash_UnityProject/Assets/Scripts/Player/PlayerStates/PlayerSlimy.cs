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

	public int health = 1;
	public float offBoundOffset = 2f;
	// Components
	public Rigidbody2D m_Rigidbody { get; set; }

	// States
	public StateMoving StateMoving { get; set; }
	public StateAiming StateAiming { get; set; }
	public StateKilled StateKilled { get; set; }
	public StateRevive StateRevive { get; set; }

	private PlayerState currentState;

	private Camera cam;
	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		cam = Camera.main;
		m_Rigidbody = GetComponent<Rigidbody2D>(); 
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Initialize States
		StateMoving = GetComponent<StateMoving>();
		StateAiming = GetComponent<StateAiming>();
		StateKilled = GetComponent<StateKilled>();
		StateRevive = GetComponent<StateRevive>();

		// Set current state
		currentState = StateMoving;
		currentState.Enter();

	}

	void Update ()
	{
		CheckForHealth();
		CheckPlayerBoundarie();
		currentState.HandleInput();
		currentState.UpdateState();
	}

	void CheckPlayerBoundarie()
	{
		// Cameras lower edge 
		float offBound = cam.transform.position.y - cam.orthographicSize - offBoundOffset;

		// Check if player is below camera's view
		if (transform.position.y < offBound)
		{
			KillPlayer();
			
			//Vector3 effectPos = new Vector3(transform.position.x, cam.transform.position.y - cam.orthographicSize, 0);
			//Instantiate(offScreenDeathEffect, effectPos, Quaternion.identity);
			//KillPlayer();
		}
	}

	void CheckForHealth()
	{
		if (health <= 0)
		{
			KillPlayer();
		}
	}

	void KillPlayer()
	{
		currentState.Exit();
		StateKilled.Enter();
		SetState(StateKilled);
	}

	public void SetState(PlayerState state)
	{
		currentState = state;
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();

		if (enemy != null)
		{
			KillPlayer();
			Debug.Log("Collided with Enemy");
			//playerState = PlayerState.KILLED;
		}
	}
}
