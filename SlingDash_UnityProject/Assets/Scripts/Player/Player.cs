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

	public GameObject forceDir;
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
    private Rigidbody2D rb;
    private Vector3 mousePos;
    private Vector2 dir;
    private float energyBarValue;

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
        rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
        analogStick.SetActive(false);
        forceDir.SetActive(false);
        onBulletTime = false;
        energyBarValue = 100;
        playerState = PlayerState.MOVING;

		spriteRenderer = GetComponent<SpriteRenderer>();
		playerDefault = spriteRenderer.sprite;
    }

	private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Player Finite State Machine
        PlayerFSM(playerState);

		CheckHealth();
		CheckPlayerOffBounds();
    }

	private void PlayerFSM(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.MOVING:
				if (energyBarValue > 0)
				    OnPlayerTap();
				spriteRenderer.sprite = playerDefault;
                break;
            case PlayerState.AIMING:
                OnPlayerHold();
				spriteRenderer.sprite = playerOnHold;
				break;
            case PlayerState.GOT_HURT:
                // Not enabled for 1 shot 1 kill on player
                break;
            case PlayerState.KILLED:
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Store Vector2 position, where player´s finger touches the screen
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            analogStick.transform.position = new Vector2(mousePos.x, mousePos.y);
			stick.transform.position = new Vector2(mousePos.x, mousePos.y);
			analogStick.SetActive(true);

            forceDir.transform.position = new Vector2(transform.position.x, transform.position.y);
            forceDir.SetActive(true);
            forceDir.transform.localScale = new Vector3(0.2f, 0f, 0);

			playerState = PlayerState.AIMING;
        }
    }

	private void OnPlayerHold()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
			SetBulletTime(true);
			SetDirection();
            SetForce();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // DISABLE BULLET TIME
            SetBulletTime(false);
            Shoot();
            playerState = PlayerState.MOVING;
        }
    }

	private void SetDirection()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        dir = new Vector2(
            mousePos.x - analogStick.transform.position.x,
            mousePos.y - analogStick.transform.position.y
        );

        analogStick.transform.up = -dir;

        forceDir.transform.up = dir;
        transform.up = -dir;

        forceDir.transform.position = transform.position;

		Vector2 stickPos = new Vector2(mousePos.x, mousePos.y);

		if (Vector2.Distance(analogStick.transform.position, stickPos) < digitalAnalogLimit)
			stick.transform.position = stickPos;

    }

	private void SetForce()
    {
        float forceAmount = Vector2.Distance(mousePos, analogStick.transform.position);

        if (forceAmount > maxThrowForceLength)
            forceAmount = maxThrowForceLength;

        forceDir.transform.localScale = new Vector3(0.2f, forceAmount, 0);
        //throwForce = forceAmount;
    }

	private void Shoot()
    {
        rb.velocity = Vector2.zero; // Resets player velocity

        rb.AddForce(transform.up * throwForce); // * forceMultiplier);
        MakeStuffDisappear();

        // SHOOT BULLET
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
            Time.timeScale = Mathf.Lerp(Time.timeScale, bulletTimeFactor, lerpBulletTime * Time.deltaTime);
			Time.fixedDeltaTime = Time.timeScale * .02f; // 1/50 = 0.02 Assuming game runs at a fixed rate of 50fps
        }
        else
        {
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

    // TURN INTO IENUMERATOR
    private void KillPlayer()
    {
		MakeStuffDisappear();

        if (GameManager.GetInstance() != null)
        {
            GameManager gm = GameManager.GetInstance();
            gm.SetState(GameManager.GameState.GAME_OVER);
        }
        Destroy(gameObject);
    }

    private void UseBulletTimeEnergy()
	{
		if (energyBarValue <= 0)
			energyBarValue = 0;
		else
			energyBarValue -= energyCostPerJump;

		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}
	
	// PROTOTYPE MODE ONLY
	private void MakeStuffDisappear()
	{
		analogStick.SetActive(false);
		forceDir.SetActive(false);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag == "TutorialTrigger")
		{
			GameManager.GetInstance().TutorialComplete();
			GameManager.GetInstance().LevelComplete();
		}

		if (collision.gameObject.tag == "LevelComplete")
            GameManager.GetInstance().LevelComplete();
    }

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	public void RechargeEnergyBar(int rechargeValue)
	{
		if (energyBarValue + rechargeValue >= 100)
			energyBarValue = 100;
		else
			energyBarValue += rechargeValue;

		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}
}
