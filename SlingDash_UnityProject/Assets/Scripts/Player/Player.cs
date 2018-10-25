using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Digital analog stick
    public GameObject analogStick;
	public GameObject stick;
    public float digitalAnalogLimit = 1f;

	// Player tail
	public GameObject tail;
	private SpriteRenderer tailSpriteRenderer;
	public Sprite tailDefault;
	public Sprite tailStreched;

    // Feedback for player to show how far Slimy will travel
    // based on finger drag distance
    public GameObject futurePosition;

	// AIMING DOTED LINE
	public GameObject dotedLine;
	public float dotedLineOffsetPos;

	public GameObject playerBulletPrefab;
	public GameObject deathEffect;
	public Sprite playerOnHold;
	public int health = 1;
	public int energyCostPerJump = 20;
    public float bulletTimeFactor = 0.02f;
	public float lerpBulletTime = 0.125f;
	public float offBoundOffset = 2f;
    public bool energyBarEnabled = false;

    public float throwForce = 300f;
    public float maxThrowForceLength = 2f;
    public float forceMultiplier = 100f;

	private Camera cam;
	private CameraShake cameraShake;

    private Rigidbody2D rb;
    private Vector3 mousePos;
    private Vector2 dir;
    private float energyBarValue;

	private bool playerKilled;

	private SpriteRenderer spriteRenderer;
	private Sprite playerDefault;


	private bool onBulletTime;
    public bool OnBulletTime() { return onBulletTime; }

    public PlayerState playerState;
    public enum PlayerState
    {
        MOVING,
        AIMING,
        GOT_HURT,
        KILLED
    }

    private void Start()
    {
		cam = Camera.main;
		cameraShake = cam.GetComponent<CameraShake>();

		rb = GetComponent<Rigidbody2D>();
        analogStick.SetActive(false);
        onBulletTime = false;
        energyBarValue = 100;

		spriteRenderer = GetComponent<SpriteRenderer>();
		playerDefault = spriteRenderer.sprite;

		tailSpriteRenderer = tail.GetComponent<SpriteRenderer>();

		playerKilled = false;

		playerState = PlayerState.MOVING;

        futurePosition.SetActive(true);

		dotedLine.SetActive(false);

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
				    OnPlayerTap();
				spriteRenderer.sprite = playerDefault;
				tailSpriteRenderer.sprite = tailDefault;
                break;
            case PlayerState.AIMING:
                OnPlayerHold();
				spriteRenderer.sprite = playerOnHold;
				tailSpriteRenderer.sprite = tailStreched;
                break;
            case PlayerState.KILLED:
				if (!playerKilled)
					KillPlayer();
                break;
        }
    }

	private void CheckPlayerOffBounds()
	{
		float offBound = cam.transform.position.y - cam.orthographicSize - offBoundOffset;
		if (transform.position.y < offBound)
		{
			Vector3 effectPos = new Vector3(transform.position.x, cam.transform.position.y - cam.orthographicSize, 0);
			Instantiate(deathEffect, effectPos, Quaternion.identity);
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
            analogStick.transform.position = new Vector2(mousePos.x, mousePos.y);
			stick.transform.position = new Vector2(mousePos.x, mousePos.y);
			analogStick.SetActive(true);

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

		// Make the stick from the digital josytick, follow the player's finger position
		Vector2 stickPos = new Vector2(mousePos.x, mousePos.y);

		// Follow finger's position if the distance is lower than de digitalAnalogLimit
		if (Vector2.Distance(analogStick.transform.position, stickPos) < digitalAnalogLimit)
		{
			stick.transform.position = stickPos;
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
        float dragLength = Vector2.Distance(mousePos, analogStick.transform.position);

        // Set a max to the drag length
        if (dragLength > maxThrowForceLength)
        {
            dragLength = maxThrowForceLength;
        }

        // Adjust tail's length in relation to drag's length
        tail.transform.localScale = new Vector3(1, dragLength, 0);

		// Store drag's length as a moving force to use later for Shoot()
		// maxThrowForce = 100%
		// dragLength = throwForce%
		throwForce = (dragLength * 100) / maxThrowForceLength;

        // Calculate player´s future position for feedback
        Vector2 futureDistanceTravelled = new Vector2(
                                            0,																	// Keep it centered  on X axis
                                            dragLength * maxThrowForceLength);		// Get force that will be aplied to Slimy

		// Set the position of the crosshair
        futurePosition.transform.localPosition = futureDistanceTravelled;

		// Enable aiming doted line
		dotedLine.SetActive(true);
		dotedLine.transform.localPosition = new Vector2(dotedLine.transform.localPosition.x,					// Keep it centered on the X axis
														-dragLength - dotedLineOffsetPos);		// Lower the sprite on the local Y axis based on dragLength
	}

	private void Shoot()
    {
		// Reset player velocity
		rb.velocity = Vector2.zero;

		// Slingshot player in his local UP direction
		throwForce *= forceMultiplier;
        rb.AddForce(transform.up * throwForce);

		// Disables some game objects 
        MakeStuffDisappear();

        // Instantiate bullet
        GameObject playerBullet = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
        playerBullet.transform.up = dir;

		// Reduces energy bar value
        if (energyBarEnabled)
		    UseBulletTimeEnergy();

		//-------------- UNCOMMENT TO ENABLE PHONE VIBRATIONS ----------------//
		//Handheld.Vibrate();
	}

	private void SetBulletTime(bool bulletTimeActive)
    {
        onBulletTime = bulletTimeActive;

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

		// CAMERA SHAKE
		StartCoroutine(cameraShake.Shake());

		// Disable some gameObjects
		MakeStuffDisappear();
        futurePosition.SetActive(false);

        // Set GameState to GAME_OVER
        if (GameManager.GetInstance() != null)
        {
            GameManager gm = GameManager.GetInstance();
            gm.SetState(GameManager.GameState.GAME_OVER);
        }

		// Disable player's gameObject, instead of destroying it
        gameObject.SetActive(false);
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
