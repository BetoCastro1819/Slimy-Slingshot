﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Effects
	public GameObject offScreenDeathEffect;
	public GameObject standardDeathEffect;

	// Digital analog stick
	public GameObject analogStick;
	public GameObject stick;
    public float digitalAnalogLimit = 1f;

	// Player tail
	public GameObject tail;
	private SpriteRenderer tailSpriteRenderer;
	public Sprite tailDefault;
	public Sprite tailStreched;
	public float tailStretchFactor = 5f;

    // Feedback for player to show how far Slimy will travel
    // based on finger drag distance
    public GameObject futurePosition;
	public float crosshairDistanceReducer = 50;

	// AIMING DOTED LINE
	public GameObject dotedLine;
	public float dotedLineOffsetPos;

	// SHOOTING RELATED VARIABLES
	public GameObject playerBulletPrefab;
	public Sprite playerOnHold;
	public int health = 1;
	public int energyCostPerJump = 20;
	public float maxBulletScale = 2f;
	public float minBulletScale = 0.5f;
	public float minDragLengthForBulletSize = 1f;
    public float bulletTimeFactor = 0.02f;
	public float lerpBulletTime = 0.125f;
	public float offBoundOffset = 2f;
    public bool energyBarEnabled = false;

    public float throwForce = 300f;
    public float maxThrowForceLength = 2f;
    public float forceMultiplier = 100f;

	/* POWER UPS */
	private CircleCollider2D circleCollider;

	// DASH
	public GameObject clearScreenTrigger;
	public float dashVelocity = 100f;
	public float metersToDash = 50;
	private float currentPosY;
	private float futurePosY;

	private Camera cam;
	private CameraShake cameraShake;

    private Rigidbody2D rb;
    private Vector3 mousePos;
    private Vector2 dir;
    private float energyBarValue;

	private bool playerKilled;

	private SpriteRenderer spriteRenderer;
	private Sprite playerDefault;

	private float dragLength;

    public bool OnBulletTime { get; set; }

	public PlayerState playerState { get; set; }
    public enum PlayerState
    {
        MOVING,
        AIMING,
        GOT_HURT,
        KILLED,
		ON_DASH
    }

    private void Start()
    {
		cam = Camera.main;
		cameraShake = cam.GetComponent<CameraShake>();

		rb = GetComponent<Rigidbody2D>();
        analogStick.SetActive(false);
        OnBulletTime = false;
        energyBarValue = 100;

		spriteRenderer = GetComponent<SpriteRenderer>();
		playerDefault = spriteRenderer.sprite;

		tailSpriteRenderer = tail.GetComponent<SpriteRenderer>();

		playerKilled = false;

		playerState = PlayerState.MOVING;

        futurePosition.SetActive(true);

		dotedLine.SetActive(false);

		//clearScreenTrigger.SetActive(false);

		currentPosY = transform.position.y;
		futurePosY = 0;

		circleCollider = GetComponent<CircleCollider2D>();
		circleCollider.enabled = true;

		//DebugScreen.Get().AddButton("Add speed", AddSpeed);
	}

	private void Update()
    {
		// Return if game is PAUSED
		if (GameManager.GetInstance() != null)
		{
			if (GameManager.GetInstance().GetState() == GameManager.GameState.PAUSE)
				return;
		}

		// Player Finite State Machine
		PlayerFSM(playerState);

		if (!playerKilled)
		{
			CheckHealth();
			CheckPlayerOffBounds();
		}
	}

	private void PlayerFSM(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.MOVING:
				if (energyBarValue > 0)
				{
					OnPlayerTap();
				}
				spriteRenderer.sprite = playerDefault;
				tailSpriteRenderer.sprite = tailDefault;
				currentPosY = transform.position.y;
                break;
            case PlayerState.AIMING:
                OnPlayerHold();
				spriteRenderer.sprite = playerOnHold;
				tailSpriteRenderer.sprite = tailStreched;
                break;
            case PlayerState.KILLED:
				if (!playerKilled)
				{
					KillPlayer();
				}
				break;
			case PlayerState.ON_DASH:
				Dash();
				break;
        }
    }

	private void CheckPlayerOffBounds()
	{
		// Cameras lower edge 
		float offBound = cam.transform.position.y - cam.orthographicSize - offBoundOffset;

		// Check if player is below camera's view
		if (transform.position.y < offBound)
		{
			Vector3 effectPos = new Vector3(transform.position.x, cam.transform.position.y - cam.orthographicSize, 0);
			Instantiate(offScreenDeathEffect, effectPos, Quaternion.identity);
			KillPlayer();
		}
	}

	private void CheckHealth()
	{
		if (health <= 0)
			playerState = PlayerState.KILLED;
	}

	private void OnPlayerTap()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))				// PLAYER TAPS THE SCREEN
        {
            // Store Vector2 position, where player´s finger touches the screen
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

			// Enable the analog stick on TAP position
			// Display the analog stick on finger's position
            analogStick.transform.position = new Vector2(mousePos.x, mousePos.y);
			stick.transform.position = new Vector2(mousePos.x, mousePos.y);
			analogStick.SetActive(true);

			// Enable crosshair to know how far the player will be slingshoted
			futurePosition.SetActive(true);
			// Enable handle on player's position
			// This will store the angle of shooting based on the player's finger position
            tail.transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
            tail.SetActive(true);
            tail.transform.localScale = new Vector3(0.5f, 0f, 0);

			playerState = PlayerState.AIMING;
        }
    }

	private void OnPlayerHold()
    {
        if (Input.GetKey(KeyCode.Mouse0))                   // PLAYER HOLDS FINGER ON SCREEN
		{
			SetBulletTime(true);
			SetDirection();
            SetForce();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))            // PLAYER RELEASES FINGER FROM SCREEN
		{
            // DISABLE BULLET TIME
            SetBulletTime(false);
            Shoot();
            playerState = PlayerState.MOVING;
        }
    }

	private void SetDirection()
    {
		// Get player's finger position
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		// Get the direction from curretn player's finger position
		// to the center of the analogStick
		dir = mousePos - analogStick.transform.position;
		// Point to finger position
        dir = new Vector2(
            mousePos.x - analogStick.transform.position.x,
            mousePos.y - analogStick.transform.position.y
        );

		// If the player's finger is at the same position as the analog stick
		// Set shooting direction to be "down" as default
		if (dir.x == 0 && dir.y == 0)
		{
			dir = -transform.up;
		}

        analogStick.transform.up = -dir;

        tail.transform.right = dir;
        transform.up = -dir;

		tail.transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

		// Make the stick from the digital josytick, follow the player's finger position
		Vector2 stickPos = new Vector2(mousePos.x, mousePos.y);

		// Follow finger's position if the distance is lower than de digitalAnalogLimit
		if (Vector2.Distance(analogStick.transform.position, stickPos) < digitalAnalogLimit)
		{
			stick.transform.position = stickPos;
		}
		// If the finger's drag is faster than the frame rate
		// set stick's position at max distance
		else
		{
			stick.transform.localPosition = new Vector2(0,
														-digitalAnalogLimit / 2);
		}

		// If the player's finger doesn't move from the center of the analogStick
		// Set dir to be down as default
		if (mousePos.x == analogStick.transform.position.x &&
			mousePos.y == analogStick.transform.position.y)
		{
			dir = -Vector3.up;
		}

		// Rotate analogStick based on direction 
		analogStick.transform.up = -dir;

		// Rotate Slimy based on direction 
		transform.up = -dir;
	}

	private void SetForce()
    {
		// Get player's drag length from center of the analogStick to finger's position
        dragLength = Vector2.Distance(mousePos, analogStick.transform.position);

        // Set a max to the drag length
        if (dragLength > maxThrowForceLength)
        {
            dragLength = maxThrowForceLength;
        }

        // Adjust tail's length in relation to drag's length
        tail.transform.localScale = new Vector3(1, dragLength / tailStretchFactor, 0);

		// Store drag's length as a moving force to use later for Shoot()
		// maxThrowForce = 100%
		// dragLength = throwForce%
		throwForce = (dragLength * 100) / maxThrowForceLength;

        // Calculate player´s future position for feedback
        Vector2 futureDistanceTravelled = new Vector2(
                                            0,																	// Keep it centered  on X axis
                                            throwForce / crosshairDistanceReducer);		// Get force that will be aplied to Slimy

		// Set the position of the crosshair
        futurePosition.transform.localPosition = futureDistanceTravelled;

		// Enable aiming doted line
		dotedLine.SetActive(true);
		dotedLine.transform.localPosition = new Vector2(dotedLine.transform.localPosition.x,					// Keep it centered on the X axis
														(-dragLength / tailStretchFactor) - dotedLineOffsetPos);		// Lower the sprite on the local Y axis based on dragLength
        tail.transform.localScale = new Vector3(dragLength, 1, 0);
        throwForce = dragLength;
    }

	private void Shoot()
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
			float bulletScale = (throwForce * maxBulletScale) / 100;
			playerBullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
			trail.startWidth = bulletScale;
		}
		else
		{
			playerBullet.transform.localScale = new Vector3(minBulletScale, minBulletScale, minBulletScale);
			trail.startWidth = minBulletScale;
		}

		playerBullet.transform.up = dir;

		// Reset player velocity
		rb.velocity = Vector2.zero;

		// Slingshot player in his local UP direction
		throwForce *= forceMultiplier;
        rb.AddForce(transform.up * throwForce);

		// Disables some game objects 
        rb.AddForce(transform.up * throwForce * forceMultiplier);
        MakeStuffDisappear();


		// Reduces energy bar value
        if (energyBarEnabled)
		    UseBulletTimeEnergy();

		//-------------- UNCOMMENT TO ENABLE PHONE VIBRATIONS ----------------//
		//Handheld.Vibrate();
	}

	private void SetBulletTime(bool bulletTimeActive)
    {
        OnBulletTime = bulletTimeActive;

        if (bulletTimeActive)
        {
			// Smooth transition from normal to slow-mo timeScale
            Time.timeScale = Mathf.Lerp(Time.timeScale, bulletTimeFactor, lerpBulletTime * Time.deltaTime);
			Time.fixedDeltaTime = Time.timeScale * .02f; // 1/50 = 0.02 Assuming game runs at a fixed rate of 50fps
        }
		else
        {
			// Resets timeScales to default
            Time.timeScale = 1;
			Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerState = PlayerState.KILLED;
        }
    }

    private void KillPlayer()
    {
		//playerKilled = true;

		// Spawn death particles
		Instantiate(standardDeathEffect, transform.position, Quaternion.identity);

		// CAMERA SHAKE
		StartCoroutine(cameraShake.Shake());

		// Disable some gameObjects
		MakeStuffDisappear();

        // Set GameState to GAME_OVER
        if (GameManager.GetInstance() != null)
        {
            GameManager gm = GameManager.GetInstance();
            gm.SetState(GameManager.GameState.GAME_OVER);
        }

		// Disable player's gameObject, instead of destroying it
        gameObject.SetActive(false);
    }

	private void Dash()
	{
		//clearScreenTrigger.SetActive(true);

		Time.timeScale = 1;

		circleCollider.enabled = false;

		transform.position += Vector3.up * dashVelocity * Time.deltaTime;

		futurePosY = currentPosY + metersToDash;
		if (transform.position.y >= futurePosY)
		{
			//clearScreenTrigger.SetActive(false);
			rb.AddForce(Vector2.up * 1000f);
			circleCollider.enabled = true;
			playerState = PlayerState.MOVING;
		}
	}

    private void UseBulletTimeEnergy()
	{
		// Decrements energyBar value if it's higher than 0
		if (energyBarValue <= 0)
		{
			energyBarValue = 0;
		}
		else
		{
			energyBarValue -= energyCostPerJump;
		}

		// Update energy bar's value on the player's UI
		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}
	
	// PROTOTYPE MODE ONLY
	private void MakeStuffDisappear()
	{
		analogStick.SetActive(false);
		dotedLine.SetActive(false);
		tail.transform.localScale = new Vector3(1, 1, 1);
        futurePosition.SetActive(false);
		futurePosition.transform.position = transform.position;
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "LevelComplete")
            GameManager.GetInstance().LevelComplete();
    }

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	public void RechargeEnergyBar(int rechargeValue)
	{
		// Recharges the energyBar only if it's value is lower than 100
		if (energyBarValue + rechargeValue >= 100)
		{
			energyBarValue = 100;
		}
		else
		{
			energyBarValue += rechargeValue;
		}

		// Update energy bar's value on player's UI
		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}

    // DEBUG SCRIPT
    void AddSpeed()
    {
        throwForce += 100;
    }
}
