using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject aimHandle;
	public GameObject forceDir;
	public float maxThrowForceLength = 2f;
	public float forceMultiplier = 100f;

	private Rigidbody2D rb;
	private Vector3 mousePos;
	private Vector2 dir;
	private float throwForce;
	private bool onBulletTime;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aimHandle.SetActive(false);
		forceDir.SetActive(false);
		onBulletTime = false;
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.Mouse0))
			SetDirection();

		if (Input.GetKeyDown(KeyCode.Mouse0))
			CreateAimingHandle();

		if (Input.GetKeyUp(KeyCode.Mouse0))
			Slingshot();
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
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		dir = new Vector2(
			mousePos.x - aimHandle.transform.position.x,
			mousePos.y - aimHandle.transform.position.y
		);

		aimHandle.transform.up = -dir;

		forceDir.transform.rotation = aimHandle.transform.rotation;
		transform.rotation = forceDir.transform.rotation;

		forceDir.transform.position = transform.position;

		SetForce();

		// Set bullet time to true
		onBulletTime = true;

		// If player collides, it keeps rotating after slingshot release
		//rb.rotation = 0;

		// Ask how to make slow mo effect non-gittery
		//Time.timeScale = 0.1f;
	}

	void SetForce()
	{
		float forceAmount = Vector2.Distance(mousePos, aimHandle.transform.position);

		if (forceAmount > maxThrowForceLength)
			forceAmount = maxThrowForceLength;

		forceDir.transform.localScale = new Vector3(0.1f, forceAmount, 0);
		throwForce = forceAmount;
		rb.velocity = Vector2.zero;

	}

	void Slingshot()
	{
		//Time.timeScale = 1f;
		rb.AddForce(transform.up * throwForce * forceMultiplier);
		aimHandle.SetActive(false);
		forceDir.SetActive(false);

		// Set bullet time to false;
		onBulletTime = false;
	}

	public bool OnBulletTime()
	{
		return onBulletTime;
	}
}
