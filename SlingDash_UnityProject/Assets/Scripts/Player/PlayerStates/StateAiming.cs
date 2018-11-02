using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAiming : PlayerState
{
	[Header("Player Sprite")]
	public Sprite playerAiming;

	[Header("Tail")]
	public GameObject tail;
	public Sprite tailStreched;
	public Sprite tailDefault;
	public float tailStretchFactor = 5f;

	[Header("Analog Stick")]
	public GameObject analogStick;
	public GameObject stick;
	public float digitalAnalogLimit = 1f;

	[Header("Aiming UI")]
	public GameObject aimingCrosshair;
	public float crosshairDistanceFactor;
	public GameObject dotedLine;
	public float dotedLineOffsetPos;

	[Header("Slingshot")]
	public float maxDragLength;
	public float slingshotForce;
	public float forceMultiplier;

	[Header("Player Bullet")]
	public GameObject playerBulletPrefab;
	public float minDragLengthForBulletSize;
	public float minBulletScale;
	public float maxBulletScale;


	private SpriteRenderer m_sRenderer;
	private Vector3 m_MousePos;
	private Vector2 m_ShootingDir;
	private float dragLength;

	public override void Start()
	{
		base.Start();

		m_sRenderer = GetComponent<SpriteRenderer>();
		Debug.Log("StateAiming.Start()");
	}

	public override void Enter()
	{
		base.Enter();

		player.SetSprite(playerAiming);

		m_MousePos = Input.mousePosition;
		m_MousePos = Camera.main.ScreenToWorldPoint(m_MousePos);

		analogStick.transform.position = new Vector2(m_MousePos.x, m_MousePos.y);
		analogStick.SetActive(true);

		dotedLine.SetActive(true);
		aimingCrosshair.SetActive(true);

		Debug.Log("StateAiming.Enter()");
	}

	public override void HandleInput()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			Debug.Log("StateAiming -> StateMoving");

			Exit();
			player.stateMoving.Enter();
			player.SetState(player.stateMoving);
		}

		base.HandleInput();
	}

	public override void UpdateState()
	{
		base.UpdateState();

		SetDirection();
		SetForce();
	}

	public override void Exit()
	{
		base.Exit();

		analogStick.SetActive(false);

		dotedLine.SetActive(false);
		aimingCrosshair.SetActive(false);

		Shoot();

		Debug.Log("StateAiming.Exit()");
	}

	void SetDirection()
	{
		// Get player's finger position
		m_MousePos = Input.mousePosition;
		m_MousePos = Camera.main.ScreenToWorldPoint(m_MousePos);

		// Get the direction from curretn player's finger position
		// to the center of the analogStick
		m_ShootingDir = m_MousePos - analogStick.transform.position;
		
		// If the player's finger is at the same position as the analog stick
		// Set shooting direction to be "down" as default
		if (m_ShootingDir.x == 0 && m_ShootingDir.y == 0)
		{
			m_ShootingDir = -transform.up;
		}

		analogStick.transform.up = -m_ShootingDir;
		transform.up = -m_ShootingDir;

		// Make the stick from the digital josytick, follow the player's finger position
		Vector2 stickPos = m_MousePos;

		// Follow finger's position if the distance is lower than de digitalAnalogLimit
		if (Vector2.Distance(analogStick.transform.position, stickPos) < digitalAnalogLimit)
		{
			stick.transform.position = stickPos;
		}
		// If the finger's drag is faster than the frame rate
		// set stick's position at max distance
		else
		{
			stick.transform.localPosition = new Vector2(0, -digitalAnalogLimit);
		}

		// Rotate analogStick based on direction 
		analogStick.transform.up = -m_ShootingDir;

		// Rotate Slimy based on direction 
		transform.up = -m_ShootingDir;
	}

	void SetForce()
	{
		// Get player's drag length from center of the analogStick to finger's position
		dragLength = Vector2.Distance(m_MousePos, analogStick.transform.position);

		// Set a max to the drag length
		if (dragLength > maxDragLength)
		{
			dragLength = maxDragLength;
		}

		// Adjust tail's length in relation to drag's length
		tail.transform.localScale = new Vector3(1, dragLength / tailStretchFactor, 0);

		// Store drag's length as a moving force to use later for Shoot()
		// maxThrowForce = 100%
		// dragLength = throwForce%
		slingshotForce = (dragLength * 100) / maxDragLength;

		// Calculate player´s future position for feedback
		Vector2 futureDistanceTravelled = new Vector2(0,														// Keep it centered  on X axis
													  slingshotForce / crosshairDistanceFactor);				// Get force that will be aplied to Slimy

		// Set the position of the crosshair
		aimingCrosshair.transform.localPosition = futureDistanceTravelled;

		// Enable aiming doted line
		dotedLine.SetActive(true);
		dotedLine.transform.localPosition = new Vector2(dotedLine.transform.localPosition.x,					// Keep it centered on the X axis
													   (-dragLength / tailStretchFactor) - dotedLineOffsetPos); // Lower the sprite on the local Y axis based on dragLength

		//tail.transform.localScale = new Vector3(dragLength, 1, 0);
		slingshotForce = dragLength;
	}

	void Shoot()
	{
		// Instantiate bullet
		GameObject playerBullet = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);

		// Get trail's component inside trail GameObject
		// Which is a child from playerBullet
		GameObject playerBulletTrail = playerBullet.GetComponent<PlayerBullet>().trail;
		TrailRenderer trail = playerBulletTrail.GetComponent<TrailRenderer>();

		// Calculate bullet scale based on player's finger drag distance
		// maxBulletScale = 100%
		// bulletScale = throwForce%
		if (dragLength > minDragLengthForBulletSize)
		{
			float bulletScale = (slingshotForce * maxBulletScale) / 100;
			playerBullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
			trail.startWidth = bulletScale;
		}
		else
		{
			playerBullet.transform.localScale = new Vector3(minBulletScale, minBulletScale, minBulletScale);
			trail.startWidth = minBulletScale;
		}

		playerBullet.transform.up = m_ShootingDir;

		// Reset player velocity
		player.m_Rigidbody.velocity = Vector2.zero;

		// Slingshot player in his local UP direction
		slingshotForce *= forceMultiplier;
		player.m_Rigidbody.AddForce(transform.up * slingshotForce * forceMultiplier);

		//-------------- UNCOMMENT TO ENABLE PHONE VIBRATIONS ----------------//
		//Handheld.Vibrate();
	}
}
