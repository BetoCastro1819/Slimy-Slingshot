using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingDash : MonoBehaviour
{
	public GameObject aimHandle;
	public GameObject forceSprite;
	public float maxThrowForceLength = 2f;
	public float forceMultiplier = 100f;

	Rigidbody2D rb;
	Vector3 mousePos;
	float throwForce;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		aimHandle.SetActive(false);
		forceSprite.SetActive(false);
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

	void SetDirection()
	{
		mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 dir = new Vector2(
			mousePos.x - aimHandle.transform.position.x,
			mousePos.y - aimHandle.transform.position.y
		);

		aimHandle.transform.up = -dir;

		transform.rotation = aimHandle.transform.rotation;



		// SetForce();
		float forceAmount = Vector2.Distance(mousePos, aimHandle.transform.position);

		if (forceAmount > maxThrowForceLength)
			forceAmount = maxThrowForceLength;

		forceSprite.transform.localScale = new Vector3(0.1f, forceAmount, 0);
		forceSprite.transform.up = dir;



		throwForce = forceAmount;

		rb.velocity = Vector2.zero;

		// If player collides, it keeps rotating after slingshot release
		//rb.rotation = 0;

		// Ask how to make slow mo effect non-gittery
		//Time.timeScale = 0.1f;
	}

	void SetForce()
	{
	}

	void Slingshot()
	{
		//Time.timeScale = 1f;
		rb.AddForce(transform.up * throwForce * forceMultiplier);
		aimHandle.SetActive(false);
		forceSprite.SetActive(false);

		Debug.Log("Force to throw: " + throwForce * forceMultiplier);
	}

	void CreateAimingHandle()
	{
		aimHandle.transform.position = new Vector2(mousePos.x, mousePos.y);
		aimHandle.SetActive(true);

		forceSprite.transform.position = new Vector2(mousePos.x, mousePos.y);
		forceSprite.SetActive(true);
		forceSprite.transform.localScale = new Vector3(0.1f, 0, 0);
	}
}
