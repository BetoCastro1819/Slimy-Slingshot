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
	public Rigidbody2D PlayerRigidbody { get; set; }

	// States
	public StateMoving StateMoving { get; set; }
	public StateAiming StateAiming { get; set; }
	public StateKilled StateKilled { get; set; }

	// PowerUp states
	public StatePowerUpDash StatePowerUpDash { get; set; }

	// List of Power Ups
	private List<PlayerState> listOfPowerUps;

	private PlayerState currentState;

	private Camera cam;
	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		cam = Camera.main;
		PlayerRigidbody = GetComponent<Rigidbody2D>(); 
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Initialize list
		listOfPowerUps = new List<PlayerState>();

		// Initialize States
		StateMoving = GetComponent<StateMoving>();
		StateAiming = GetComponent<StateAiming>();
		StateKilled = GetComponent<StateKilled>();

		// Initialize PowerUp states
		StatePowerUpDash = GetComponent<StatePowerUpDash>();
		listOfPowerUps.Add(StatePowerUpDash);
		
		// Set current state
		currentState = StateMoving;
		currentState.Enter();

	}

	void Update ()
	{
		if (GameManager.GetInstance().GetState() != GameManager.GameState.PAUSE)
		{
			CheckForHealth();
			CheckPlayerBoundarie();
			currentState.HandleInput();
			currentState.UpdateState();
		}
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
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PowerUp")
		{
			// Randomly pick one of the power up states
			//int randomIndex = Random.Range(0, listOfPowerUps.Count);

			// Change state
			currentState.Exit();
			listOfPowerUps[0].Enter();
			SetState(listOfPowerUps[0]);
		}
	}
}
