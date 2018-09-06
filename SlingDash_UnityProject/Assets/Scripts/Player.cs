using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject aimHandle;
	public GameObject forceDir;
    public GameObject playerBulletPrefab;
	public float throwForce = 300f;
	public float energyBarRechargeValue = 5f;

	//public float maxThrowForceLength = 2f;
	//public float forceMultiplier = 100f;

	private Rigidbody2D rb;
	private Vector3 mousePos;
	private Vector2 dir;
	private bool onBulletTime;
	private float energyBarValue;


	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aimHandle.SetActive(false);
		forceDir.SetActive(false);
		onBulletTime = false;
		energyBarValue = 100;
	}
	
	void Update ()
	{
		if (energyBarValue > 0)
		{
			if (Input.GetKey(KeyCode.Mouse0))
				SetDirection();
			else
				RechargeEnergyBar();

			if (Input.GetKeyDown(KeyCode.Mouse0))
				CreateAimingHandle();

			if (Input.GetKeyUp(KeyCode.Mouse0))
				Slingshot();
		}
		else
			MakeStuffDisappear();
	}

	void CreateAimingHandle()
	{
		aimHandle.transform.position = new Vector2(mousePos.x, mousePos.y);
		aimHandle.SetActive(true);

		forceDir.transform.position = new Vector2(transform.position.x, transform.position.y);
		forceDir.SetActive(true);
		forceDir.transform.localScale = new Vector3(0.1f, 0, 0);
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

		forceDir.transform.localScale = new Vector3(0.1f, 1, 0);


		// UNCOMMENT TO ENABLE 
		//SetForce();

		// ON BULLET TIME
		BulletTime();


		// If player collides, it keeps rotating after slingshot release
		//rb.rotation = 0;

		// Ask how to make slow mo effect non-gittery
		//Time.timeScale = 0.1f;
	}

	void SetForce()
	{
		/*
		float forceAmount = Vector2.Distance(mousePos, aimHandle.transform.position);

		if (forceAmount > maxThrowForceLength)
			forceAmount = maxThrowForceLength;

		forceDir.transform.localScale = new Vector3(0.1f, forceAmount, 0);
		throwForce = forceAmount;
		*/
	}

	void Slingshot()
	{
		//Time.timeScale = 1f;
		rb.AddForce(transform.up * throwForce);	/* * forceMultiplier*/
		aimHandle.SetActive(false);
		forceDir.SetActive(false);

		// Set bullet time to false;
		onBulletTime = false;

        // SHOOT BULLET
        GameObject playerBullet = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
        playerBullet.transform.up = dir;
	}

	void BulletTime()
	{
		onBulletTime = true;

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

	public bool OnBulletTime()
	{
		return onBulletTime;
	}

	// PROTOTYPE MODE ONLY
	private void MakeStuffDisappear()
	{
		aimHandle.SetActive(false);
		forceDir.SetActive(false);
	}
}
