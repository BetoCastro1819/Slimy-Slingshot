using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject aimHandle;
    public GameObject forceDir;
    public GameObject playerBulletPrefab;
    public float bulletTimeFactor = 0.02f;
    public float throwForce = 300f;
    public float energyBarRechargeValue = 5f;

    public float maxThrowForceLength = 2f;
    public float forceMultiplier = 100f;

    private Rigidbody2D rb;
    private Vector3 mousePos;
    private Vector2 dir;
    private float energyBarValue;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aimHandle.SetActive(false);
        forceDir.SetActive(false);
        onBulletTime = false;
        energyBarValue = 100;
        playerState = PlayerState.MOVING;
    }

    void Update()
    {
        PlayerFSM(playerState);
    }

    void PlayerFSM(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.MOVING:
                OnPlayerTap();
                break;
            case PlayerState.AIMING:
                OnPlayerHold();
                break;
            case PlayerState.GOT_HURT:
                break;
            case PlayerState.KILLED:
                KillPlayer();
                break;
        }
    }

    void OnPlayerTap()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // ENABLE BULLET TIME
            BulletTime(true);

            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            aimHandle.transform.position = new Vector2(mousePos.x, mousePos.y);
            aimHandle.SetActive(true);

            forceDir.transform.position = new Vector2(transform.position.x, transform.position.y);
            forceDir.SetActive(true);
            forceDir.transform.localScale = new Vector3(0.1f, 0, 0);

            playerState = PlayerState.AIMING;
        }
    }

    void OnPlayerHold()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            SetDirection();
            SetForce();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // DISABLE BULLET TIME
            BulletTime(false);
            Shoot();
            playerState = PlayerState.MOVING;
        }
    }

    void SetDirection()
    {
        // Stops the player from falling
        rb.velocity = Vector2.zero;

        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        dir = new Vector2(
            mousePos.x - aimHandle.transform.position.x,
            mousePos.y - aimHandle.transform.position.y
        );

        aimHandle.transform.up = -dir;

        forceDir.transform.up = dir;
        transform.up = -dir;

        forceDir.transform.position = transform.position;
    }

    void SetForce()
    {
        float forceAmount = Vector2.Distance(mousePos, aimHandle.transform.position);

        if (forceAmount > maxThrowForceLength)
            forceAmount = maxThrowForceLength;

        forceDir.transform.localScale = new Vector3(0.2f, forceAmount, 0);

        throwForce = forceAmount;
    }

    void Shoot()
    {
        rb.AddForce(transform.up * throwForce * forceMultiplier);
        MakeStuffDisappear();

        // SHOOT BULLET
        GameObject playerBullet = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
        playerBullet.transform.up = dir;
    }

    void BulletTime(bool bulletTimeActive)
    {
        onBulletTime = bulletTimeActive;

        if (bulletTimeActive)
        {
            Time.timeScale = bulletTimeFactor;
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
            Debug.Log("morite wacho");
        }
    }

    // TURN INTO IENUMERATOR
    void KillPlayer()
    {
        Destroy(gameObject);
    }

    void EnergyBar()
	{
		if (energyBarValue <= 0)
			energyBarValue = 0;
		else
			energyBarValue -= energyBarRechargeValue;

		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}

	void RechargeEnergyBar()
	{
		if (energyBarValue >= 100)
			energyBarValue = 100;
		else
			energyBarValue += energyBarRechargeValue / 3;

		UI_Manager.Get().energyBar.value = energyBarValue / 100;
	}
	
	// PROTOTYPE MODE ONLY
	private void MakeStuffDisappear()
	{
		aimHandle.SetActive(false);
		forceDir.SetActive(false);
	}
}
